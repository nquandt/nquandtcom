using Azure.Identity;
using Azure.Storage.Blobs;
using Femur;
using Femur.FileSystem;
using Femur.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Server;
using Server.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables("ENV_");

builder.Services.AddTransient<UpdateEndpoint>();

builder.Services.TryConfigureByConventionWithValidation<StorageOptions>();

builder.Services.AddSingleton<IFileSystem>((IServiceProvider sp) =>
{
    var storageOptions = sp.GetRequiredService<IOptions<Server.StorageOptions>>().Value;

    return new AzureBlobStorageFileSystem(new BlobContainerClient(new Uri(new Uri(storageOptions.BaseUrl), "/siteoutput"), new DefaultAzureCredential()));
});

builder.Services.AddDefaultJsonSerializer(System.Text.Json.JsonSerializerOptions.Web);

builder.Services.TryConfigureByConventionWithValidation<ClientOptions>();

builder.Services.AddHttpClient("client", (IServiceProvider sp, HttpClient client) =>
{
    var options = sp.GetRequiredService<IOptions<ClientOptions>>().Value;

    client.BaseAddress = new Uri(options.BaseUrl);
});

var app = builder.Build();

app.Use(async (ctx, next) =>
{
    // ctx.SetNoCache();
    await next(ctx);
});

// app.UseHttpsRedirection();

static string? GetContentTypeFromExtension(string slug)
{
    var split = slug.Split('.');
    if (split.Length > 0)
    {
        switch (split.Last())
        {
            case "md":
                return "text/plain";
            case "js":
                return "text/javascript";
            case "json":
                return "application/json";
            case "html":
                return "text/html";
            case "ico":
                return "image/vnd.microsoft.icon";
            case "svg":
                return "image/svg+xml";
            case "css":
                return "text/css";
        }
    }

    return null;
}

app.MapGet("/{**slug}", async (IFileSystem fs, HttpContext context, CancellationToken cancellationToken, [FromRoute] string? slug = null) =>
{
    if (slug != null && await fs.FileExistsAsync(slug))
    {
        var file = await fs.OpenReadAsync(slug, cancellationToken);

        var contentType = GetContentTypeFromExtension(slug);

        context.SetMaxAge1StaleInfinite();
        return Results.Stream(file, contentType);
    }

    slug = slug is null
        ? "./index.html"
        : (slug.EndsWith("/")
                ? slug.Substring(0, slug.Length - 1)
                : slug) + "/index.html";

    if (await fs.FileExistsAsync(slug))
    {
        var file = await fs.OpenReadAsync(slug, cancellationToken);

        return Results.Stream(file, "text/html");
    }

    context.Response.StatusCode = 404;
    return Results.Stream(await fs.OpenReadAsync("./404.html", cancellationToken), "text/html");
});

UpdateEndpoint.ConfigureEndpoint(app);

app.Run();
