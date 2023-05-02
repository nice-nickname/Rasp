using Incoding.Web.MvcContrib;

namespace UI;

public class ConfirmSettings
{
    public enum ButtonColor
    {
        Primary,

        Light,

        Success,

        Warning,

        Danger
    }

    public string Id { get; set; }

    public string Url { get; set; }

    public string Text { get; set; }

    public string TextInProcess { get; set; }

    public string TextConfirm { get; set; }

    public string Icon { get; set; }

    public bool IsDisabled { get; set; }

    public ButtonColor Color { get; set; }

    public Action<IIncodingMetaLanguageCallbackBodyDsl>? OnSuccess { get; set; }
}
