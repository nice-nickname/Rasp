using Domain.Common;
using Incoding.Core.CQRS.Core;
using Resources;

namespace Domain.App.Api;

public class GetSubDisciplineIsParallelOptionsForSelectQuery : QueryBase<List<DropDownItem>>
{
    public bool Selected { get; set; }

    protected override List<DropDownItem> ExecuteResult()
    {
        return new List<DropDownItem>
        {
                new(false, DataResources.IsParallelType_Individual, !Selected),
                new(true, DataResources.IsParallelType_Parallel, Selected)
        };
    }
}
