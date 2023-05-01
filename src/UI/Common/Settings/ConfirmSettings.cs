using Incoding.Web.MvcContrib;

namespace UI;

public class ConfirmSettings
{
    public string Id { get; set; }

    public string Url { get; set; }

    public bool IsDisabled { get; set; }

    public Action<IIncodingMetaLanguageCallbackBodyDsl>? OnSuccess { get; set; }
}
