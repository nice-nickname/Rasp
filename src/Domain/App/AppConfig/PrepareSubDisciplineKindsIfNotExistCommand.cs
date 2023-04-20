using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class PrepareSubDisciplineKindsIfNotExistCommand : CommandBase
{
    protected override void Execute()
    {
        if (!Repository.Query<SubDisciplineKind>().Any())
        {
            Repository.Save(new SubDisciplineKind
            {
                    Name = "Лекция",
                    Code = "л."
            });
            Repository.Save(new SubDisciplineKind
            {
                    Name = "Практика",
                    Code = "пр."
            });
            Repository.Save(new SubDisciplineKind
            {
                    Name = "Лабораторная работа",
                    Code = "лаб."
            });
            Repository.Save(new SubDisciplineKind
            {
                    Name = "Экзамен",
                    Code = "экз."
            });
        }
    }
}
