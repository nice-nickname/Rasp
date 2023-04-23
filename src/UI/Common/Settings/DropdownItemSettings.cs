using Incoding.Web.MvcContrib;

namespace UI;

public class DropdownItemSettings
{
    public string Text { get; set; }

    public string Href { get; set; }

    public string Url { get; set; }

    public string Id { get; set; }

    public Action<IIncodingMetaLanguageCallbackBodyDsl>? OnSuccess { get; set; }
}
