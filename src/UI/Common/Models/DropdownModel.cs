using Microsoft.AspNetCore.Html;

namespace UI.Common.Models;

public class DropdownModel
{
    public string Classes { get; set; }

    public ButtonSettings? Button { get; set; }

    public DropdownMenuSettings? DropdownMenu { get; set; }

    public List<IHtmlContent> Items { get; set; }

    public class ButtonSettings
    {
        public string Text { get; set; }

        public string Title { get; set; }

        public string? Icon { get; set; }

        public string Classes { get; set; } = "btn-light";

        public bool IsDisabled { get; set; }

        public string Id { get; set; }

        public string TextId { get; set; }
    }

    public class DropdownMenuSettings
    {
        public string Classes { get; set; }
    }
}
