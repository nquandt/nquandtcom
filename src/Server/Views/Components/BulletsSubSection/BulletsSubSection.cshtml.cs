
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Razor.Templating.Core;

namespace Components;

[HtmlTargetElement("bullets-sub-section")]
public class BulletsSubSection : ViewModel<BulletsSubSection>
{
    public Func<object?, IHtmlContent> Header { get; set; } = (o) => new StringHtmlContent("");

    public required string[] Bullets { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        TagHelperContent childContent = await output.GetChildContentAsync();
        Header = (o) => childContent;
        var content = await RazorTemplateEngine.RenderPartialAsync(ViewHelper.GetViewName(this.Type), this);

        output.Content.SetHtmlContent(content);
    }
}