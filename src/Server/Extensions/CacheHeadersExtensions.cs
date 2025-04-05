
namespace Server;

public static class CacheHeaderExtensions
{
    public static void SetNoCache(this HttpContext context)
    {
        context.Response.Headers.Append("cache-control", "no-cache, no-store");
    }

    public static void SetMaxAge1StaleInfinite(this HttpContext context)
    {
        context.Response.Headers.Append("cdn-cache-control", "public, max-age=1, stale-while-revalidate=31560000");
    }
}