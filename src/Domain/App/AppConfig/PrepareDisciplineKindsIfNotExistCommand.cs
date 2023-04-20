using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class PrepareDisciplineKindsIfNotExistCommand : CommandBase
{
    protected override void Execute()
    {
        if (!Repository.Query<DisciplineKind>().Any())
        {
            Repository.Save(new DisciplineKind
            {
                    Name = "Зачет"
            });
            Repository.Save(new DisciplineKind
            {
                    Name = "Дифференцированный зачет"
            });
            Repository.Save(new DisciplineKind
            {
                    Name = "Экзамен"
            });
        }
    }
}
