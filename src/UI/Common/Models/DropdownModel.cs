namespace UI.Common.Models;

using Microsoft.AspNetCore.Html;

public class DropdownModel
{
    public string Text { get; set; }

    public string Title { get; set; }

    public List<IHtmlContent> Items { get; set; }
}
