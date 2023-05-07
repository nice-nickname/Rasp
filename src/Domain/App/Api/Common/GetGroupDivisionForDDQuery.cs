using Domain.Common;
using Incoding.Core.CQRS.Core;

namespace Domain.App.Api;

public class GetGroupDivisionForDDQuery : QueryBase<List<DropDownItem>>
{
    public int Selected { get; set; }

    protected override List<DropDownItem> ExecuteResult()
    {
        var items = new List<DropDownItem>
        {
                new(1, "Без подгрупп"),
                new(2, "2"),
                new(3, "3"),
                new(4, "4")
        };
        var find = items.FirstOrDefault(s => s.Value.Equals(Selected));
        if (find != null)
        {
            find.Selected = true;
        }
        else
        {
            items[0].Selected = true;
        }

        return items;
    }
}
