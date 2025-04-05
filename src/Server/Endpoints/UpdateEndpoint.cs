using Femur;
using Femur.FileSystem;
using Femur.Serialization;

namespace Server.Endpoints;

public class UpdateEndpoint
{
    public static void ConfigureEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapEndpoint<UpdateEndpoint>("/update", ["POST"], i => i.HandleAsync);
    }

    private readonly IFileSystem _fs;
    private readonly IAsyncSerializerFactory _serializer;
    private readonly IHttpClientFactory _factory;

    public UpdateEndpoint(IFileSystem fs, IAsyncSerializerFactory serializerFactory, IHttpClientFactory factory)
    {
        _fs = fs;
        _serializer = serializerFactory;

        _factory = factory;
    }

    public async Task<IResult> HandleAsync(HttpContext context, CancellationToken cancellationToken)
    {
        var request = await _serializer.DeserializeAsync<Dto>(context.Request.Body, "application/json", cancellationToken);

        if (request == null)
        {
            return Results.BadRequest("Please provide something plz.");
        }

        using var client = _factory.CreateClient("client");

        var assetDownloader = new AssetDownloader(client, _fs);

        // start app in proc...
        using var app = await WebAppProcess.Start(_factory, "node", ["./dist/server/entry.mjs"], 4321);

        foreach (var route in request.Routes)
        {
            await assetDownloader.DownloadSiteAsync(route);
        }

        return Results.Ok();
    }
}

public class Dto
{
    public required string[] Routes { get; set; }
}
