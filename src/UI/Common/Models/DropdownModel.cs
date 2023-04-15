using Microsoft.AspNetCore.Html;

namespace UI.Common.Models;

public class DropdownModel
{
    public string Text { get; set; }

    public string Title { get; set; }

    public List<IHtmlContent> Items { get; set; }
}
