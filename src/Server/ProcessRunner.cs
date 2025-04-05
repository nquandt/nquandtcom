using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CliWrap;

namespace Server;

public class WebAppProcess : IDisposable
{
    public static async Task<WebAppProcess> Start(IHttpClientFactory factory, string command, string[] arguments, int port = 7893, CancellationToken cancellationToken = default)
    {
        var app = new WebAppProcess(factory, command, arguments, port);

        app.Start();

        await app.WaitForHealthyAsync(TimeSpan.FromSeconds(30), cancellationToken);

        return app;
    }

    private readonly Uri _baseUri;
    private readonly IHttpClientFactory _factory;
    private readonly ProcessRunner _processRunner;

    public WebAppProcess(IHttpClientFactory factory, string command, string[] arguments, int port)
    {
        _processRunner = new ProcessRunner(command, arguments, new Dictionary<string, string?>()
        {
            ["PORT"] = port.ToString()
        });
        _factory = factory;
        _baseUri = new Uri($"http://localhost:{port}");
    }

    public void Start()
    {
        _ = Task.Run(() => _processRunner.RunAsync());
    }

    public async Task WaitForHealthyAsync(TimeSpan timeout, CancellationToken cancellationToken)
    {
        using var client = _factory.CreateClient();
        using var timeoutCts = new CancellationTokenSource(timeout);
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, cancellationToken);

        while (!linkedCts.Token.IsCancellationRequested)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(_baseUri, linkedCts.Token);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Service is healthy: {response.StatusCode}");
                    return;
                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Health check request timed out.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while checking health: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromSeconds(2), linkedCts.Token);
        }

        throw new TimeoutException($"Service did not become healthy within {timeout.TotalSeconds} seconds.");
    }

    public void Dispose()
    {
        _processRunner.Kill();
    }
}

class ProcessRunner
{
    private readonly CancellationTokenSource _forceCts;
    private readonly Command _command;

    public ProcessRunner(string command, string[] arguments, Dictionary<string, string?> envVars)
    {
        _forceCts = new CancellationTokenSource();

        _command = Cli.Wrap(command)
            .WithArguments(arguments)
            .WithEnvironmentVariables(envVars)
            .WithStandardOutputPipe(PipeTarget.ToStream(Console.OpenStandardOutput()))
            .WithStandardErrorPipe(PipeTarget.ToStream(Console.OpenStandardError()))
            ;
    }


    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        await _command.ExecuteAsync(_forceCts.Token, cancellationToken);
    }

    public void Kill()
    {
        _forceCts.Cancel();
    }
}
