using Incoding.Core.CQRS.Core;

namespace Domain.Common;

public class GetGroupDivisionForDDQuery : QueryBase<List<DropDownItem>>
{
    protected override List<DropDownItem> ExecuteResult()
    {
        return new List<DropDownItem>
        {
                new(1, "Без подгрупп", false),
                new(2, "2", false),
                new(3, "3", false),
                new(4, "4", false)
        };
    }
}
