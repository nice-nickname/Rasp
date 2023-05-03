using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;
using Resources;

namespace Domain.Api;

public class GetSubDisciplineKindTypesQuery : QueryBase<List<KeyValueVm>>
{
    public SubDisciplineKind.OfType Select { get; set; }

    protected override List<KeyValueVm> ExecuteResult()
    {
        return new List<KeyValueVm>
        { 
                new(SubDisciplineKind.OfType.LECTURE, DataResources.Lecture, Select == SubDisciplineKind.OfType.LECTURE),
                new(SubDisciplineKind.OfType.PRACTICE, DataResources.Practice, Select == SubDisciplineKind.OfType.PRACTICE),
                new(SubDisciplineKind.OfType.EXAM, DataResources.Exam, Select == SubDisciplineKind.OfType.EXAM)
        };
    }
}
