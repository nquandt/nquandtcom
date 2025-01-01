using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Components;

public class Container : ViewModel<Container>
{    
    public Func<object?, IHtmlContent> Content { get; set; } = (o) => new StringHtmlContent("");
}