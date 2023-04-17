using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class PrepareFacultyIfNotExistCommand : CommandBase
{
    protected override void Execute()
    {
        if (!Repository.Query<Faculty>().Any())
        {
            Repository.Save(new Faculty
            {
                    Name = "Институт компьютерных технологий и информационной безопасности",
                    Code = "ИКТИБ"
            });
        }
    }
}

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
