
using Microsoft.AspNetCore.Razor.TagHelpers;

public interface IViewModel
{
    Type Type { get; }
}

public abstract class ViewModel<T> : TagHelper, IViewModel where T : class
{    
    public Type Type => typeof(T);
}

// public abstract class ViewModel<T> : IViewModel<T> where T : class
// {
//     public static string ViewName => ViewHelper.GetViewName<T>();
// }