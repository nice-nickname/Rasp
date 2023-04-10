namespace UI;

using Incoding.Web.MvcContrib;

public class FormSettings
{
    public string Url { get; set; }

    public string Name { get; set; }

    public Action<IIncodingMetaLanguageCallbackBodyDsl>? OnSave { get; set; }

    public Action<IIncodingMetaLanguageCallbackBodyDsl>? OnError { get; set; }
}
