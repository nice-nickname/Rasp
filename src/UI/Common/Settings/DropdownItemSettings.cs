namespace UI;

using Incoding.Web.MvcContrib;

public class DropdownItemSettings
{
    public string Text { get; set; }

    public string Href { get; set; }

    public Action<IIncodingMetaLanguageCallbackBodyDsl>? OnSuccess { get; set; }
}
