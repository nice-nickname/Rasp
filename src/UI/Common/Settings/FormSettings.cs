using Incoding.Web.MvcContrib;

namespace UI;

public class FormSettings
{
    public string Url { get; set; }

    public string Name { get; set; }

    public string Class { get; set; }

    public Action<IIncodingMetaLanguageCallbackBodyDsl>? OnSave { get; set; }

    public Action<IIncodingMetaLanguageCallbackBodyDsl>? OnError { get; set; }
}
