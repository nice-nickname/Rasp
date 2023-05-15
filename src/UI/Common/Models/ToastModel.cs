namespace UI.Common.Models;

public class ToastModel
{
    public enum Color
    {
        Primary,

        Secondary,

        Success,

        Warning,

        Danger,

        White,

        Dark
    }

    public string Id { get; set; }

    public string Message { get; set; }

    public string Title { get; set; }

    public Color TitleColor { get; set; }

    public Color TitleTextColor { get; set; }
}
