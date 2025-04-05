using Femur.FileSystem;
using HtmlAgilityPack;

namespace Server;

public class AssetDownloader
{
    private readonly HttpClient _httpClient;
    private readonly IFileSystem _fileSystem;

    public AssetDownloader(HttpClient httpClient, IFileSystem fileSystem)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public async Task DownloadSiteAsync(string url)
    {
        var html = await _httpClient.GetStringAsync(url);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Download external resources
        await DownloadResourcesAsync(doc);

        // Save the modified HTML file
        await SaveHtmlFileAsync(html, url);
    }

    private async Task DownloadResourcesAsync(HtmlDocument doc)
    {
        var resourceNodes = doc.DocumentNode.SelectNodes("//script | //link");

        if (resourceNodes == null) return;

        foreach (var node in resourceNodes)
        {
            string? src = node.Name == "script" ? node.GetAttributeValue("src", null!) :
                          node.Name == "link" ? node.GetAttributeValue("href", null!) : null;

            if (string.IsNullOrEmpty(src))
                continue;

            try
            {
                using var resourceStream = await _httpClient.GetStreamAsync(src);

                var final = "." + src;

                var parent = final.Split('/');
                if (parent.Length > 2)
                {
                    await _fileSystem.CreateDirectoryAsync(string.Join('/', parent[0..^1]));
                }

                await _fileSystem.WriteAsync(final, resourceStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to download {src}: {ex.Message}");
            }
        }
    }

    private async Task SaveHtmlFileAsync(string document, string route)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream);
        writer.Write(document);
        writer.Flush();
        memoryStream.Position = 0;

        var folder = string.Join('/', new string[] { ".", route }.Select(x => x.Trim('/')).Where(x => !string.IsNullOrWhiteSpace(x)));

        await _fileSystem.CreateDirectoryAsync(folder);
        await _fileSystem.WriteAsync(folder + "/index.html", memoryStream);
    }

    private static string GetSafeFileName(string urlPath)
    {
        return urlPath.Replace("/", "_").Replace("?", "_").Replace("&", "_").Replace("=", "_");
    }
}
