using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Razor.Templating.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorTemplating();

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationExpanders.Add(new Expander());
    options.ViewLocationFormats.Add("/Views/{0}/_" + RazorViewEngine.ViewExtension);
    options.ViewLocationFormats.Add("/Views/{0}" + RazorViewEngine.ViewExtension);    
    options.ViewLocationFormats.Add("/Pages/{0}" + RazorViewEngine.ViewExtension);
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.Use(async (ctx, next) => {
    if (ctx.Request.Headers.TryGetValue("HX-Request", out var headers) && headers[0] == "true")
    {
        ctx.Response.Headers.Vary = "HX-Request";
        ctx.Response.Headers.CacheControl = "public, max-age=10";
    }

    await next(ctx);
});

app.MapGet("/{**slug}", async ([FromServices] IRazorTemplateEngine engine, [FromRoute] string slug = "") =>
{
    slug = slug.StartsWith('/') ? slug : "/" + slug;
    slug = slug.EndsWith('/') ? slug : slug + "/";
    //Render View From the RCL
    var (exists, renderedString) = await engine.TryRenderAsync($"/Pages{slug}index.cshtml", null);

    if (!exists)
    {
        var (_, fourOhFourPage) = await engine.TryRenderAsync($"/Pages/404/index.cshtml", null);
        
        return new HtmlResult(fourOhFourPage!, System.Net.HttpStatusCode.NotFound);
    }

    return new HtmlResult(renderedString!);
});

app.MapGet("/{fileName}.css", ([FromRoute] string fileName) =>
{
    string _basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;

    var file = Path.Join(_basePath, $"/Views/{fileName}.css");
    if (!File.Exists(file))
    {
        return Results.NotFound("File Not Found");
    }
    
    var contents = System.IO.File.ReadAllBytes(file);

    return Results.File(contents, "text/css");
});

app.Run();
