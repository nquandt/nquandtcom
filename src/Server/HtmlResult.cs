using System.Net;
using System.Net.Mime;
using System.Text;

class HtmlResult : IResult
{
    private readonly string _html;
    private readonly HttpStatusCode _httpStatusCode;

    public HtmlResult(string html, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
    {
        _html = html;
        _httpStatusCode = httpStatusCode;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int)_httpStatusCode;
        httpContext.Response.ContentType = MediaTypeNames.Text.Html;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(_html);
        return httpContext.Response.WriteAsync(_html);
    }
}