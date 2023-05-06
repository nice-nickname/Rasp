using Incoding.Core.CQRS.Core;

namespace Domain.Common;

public class GetSubDisciplineIsParallelOptionsForDDQuery : QueryBase<List<DropDownItem>>
{
    public bool Selected { get; set; }

    protected override List<DropDownItem> ExecuteResult()
    {
        return new List<DropDownItem>
        {
                new(false, "Отдельно для каждого преподавателя", !Selected),
                new(true, "Параллельно", Selected)
        };
    }
}
