﻿using Resources;

namespace Domain;

public class DropDownItem
{
    public object Value { get; set; }

    public string Text { get; set; }

    public string SubText { get; set; }

    public string Group { get; set; }

    public string Search { get; set; }

    public bool Selected { get; set; }

    public DropDownItem(object value, string text, bool selected, string group = "", string subtext = "")
    {
        this.Value = value;
        this.Text = text;
        this.SubText = string.Empty;
        this.Group = string.Empty;
        this.Selected = selected;
        if (group != "")
        {
            this.Group = group;
        }

        if (subtext != "")
        {
            this.SubText = subtext;
        }

        Search = (string.IsNullOrWhiteSpace(group) ? "" : group) + " "
                                                                 + (string.IsNullOrWhiteSpace(subtext) ? "" : subtext) + " "
                                                                 + Text + " ";
    }
}