using Microsoft.AspNetCore.Mvc.Rendering;

public static class ViewHelper
{
    public static string GetViewName<T>() where T : class
    {
        return GetViewName(typeof(T));
    }

    public static string GetViewName(Type type)
    {
        return type.FullName?.Replace('.', '/') + "/" + type.Name ?? "";
    }

    public static async Task<Microsoft.AspNetCore.Html.IHtmlContent> RenderViewAsync<T>(this IHtmlHelper htmlHelper, object model) where T : IViewModel
    {
        var viewName = GetViewName(typeof(T));

        return await htmlHelper.RenderViewAsync(viewName, model);
    }

    public static async Task<Microsoft.AspNetCore.Html.IHtmlContent> RenderViewAsync<T>(this IHtmlHelper htmlHelper, T? model = null) where T : ViewModel<T>
    {
        var viewName = GetViewName<T>();

        return await htmlHelper.RenderViewAsync(viewName, model);
    }

    public static async Task<Microsoft.AspNetCore.Html.IHtmlContent> RenderDynamicAsync(this IHtmlHelper htmlHelper, IViewModel model)
    {
        var viewName = GetViewName(model.Type);

        return await htmlHelper.RenderViewAsync(viewName, model);
    }

    public static async Task<Microsoft.AspNetCore.Html.IHtmlContent> RenderDynamicAsync(this IHtmlHelper htmlHelper, object model)
    {
        var viewName = GetViewName(model.GetType());

        return await htmlHelper.RenderViewAsync(viewName, model);
    }

    private static async Task<Microsoft.AspNetCore.Html.IHtmlContent> RenderViewAsync(this IHtmlHelper htmlHelper, string viewName, object? model)
    {
        return await htmlHelper.PartialAsync(viewName, model);
    }
}