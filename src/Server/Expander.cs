using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;

class Expander : IViewLocationExpander
{
    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        context.Values.TryGetValue("_path", out var val);

        if (string.IsNullOrWhiteSpace(val))
        {
            return viewLocations;
        }

        return ExpandViewLocationsCore(viewLocations, val);
    }

    private IEnumerable<string> ExpandViewLocationsCore(IEnumerable<string> viewLocations, string value)
    {
        yield return value;
        foreach (var location in viewLocations)
        {
            yield return location;
        }
    }

    public void PopulateValues(ViewLocationExpanderContext context)
    {
        if (context.ActionContext is ViewContext vc)
        {
            if (vc.View.Path.StartsWith("/Pages") && vc.View.Path.EndsWith("index.cshtml"))
            {
                context.Values["_path"] = vc.View.Path.Replace("index.cshtml", "{0}.cshtml");
            }
        }
    }
}
