

namespace Components.Utils;

public class HtmxATag : ViewModel<HtmxATag>
{
    public HtmxATag(string href, string text)
    {
        Href = href;
        Text = text;
    }

    public string Href { get; set; }
    public string Text { get; set; }
}