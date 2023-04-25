using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class DropDownItem
{
    public object Value { get; set; }

    public string Text { get; set; }

    public string SubText { get; set; }

    public string Group { get; set; }

    public string Search { get; set; }

    public bool Selected { get; set; }

    public DropDownItem(object value, string text, bool selected, string? group = null)
    {
        this.Value = value;
        this.Text = text;
        this.SubText = string.Empty;
        this.Group = string.Empty;
        this.Selected = selected;
        if (group != null)
        {
            this.Group = group;
        }

        Search = Text + " " + (string.IsNullOrWhiteSpace(group) ? "" : group);
    }
}

public class GetGroupsForDDQuery : QueryBase<List<DropDownItem>>
{
    public int FacultyId { get; set; }

    public int[]? SelectedIds { get; set; }

    protected override List<DropDownItem> ExecuteResult()
    {
        SelectedIds ??= Array.Empty<int>();
        return Repository.Query(new Share.Where.ByFacultyThoughDepartment<Group>(FacultyId))
                         .Select(s => new DropDownItem(s.Id, s.Code, SelectedIds.Contains(s.Id), s.Department.Code))
                         .ToList();
    }
}
