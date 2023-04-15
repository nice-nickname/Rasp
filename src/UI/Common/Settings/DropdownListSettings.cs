using Incoding.Web.MvcContrib;

namespace UI;

public class DropdownListSettings
{
    public string Url { get; set; }

    public string CustomTemplate { get; set; }

    public Action<IIncodingMetaLanguageCallbackBodyDsl>? OnSuccess { get; set; }

    public Action<IIncodingMetaLanguageCallbackBodyDsl>? OnClick { get; set; }
}
