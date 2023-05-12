using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;
using Resources;

namespace Domain.Api;

public class GetSubDisciplineKindTypesQuery : QueryBase<List<KeyValueVm>>
{
    public SubDisciplineKind.OfType Selected { get; set; }

    protected override List<KeyValueVm> ExecuteResult()
    {
        return new List<KeyValueVm>
        { 
                new(SubDisciplineKind.OfType.LECTURE, DataResources.Lecture, Selected == SubDisciplineKind.OfType.LECTURE),
                new(SubDisciplineKind.OfType.PRACTICE, DataResources.Practice, Selected == SubDisciplineKind.OfType.PRACTICE),
                new(SubDisciplineKind.OfType.EXAM, DataResources.Exam, Selected == SubDisciplineKind.OfType.EXAM)
        };
    }
}
