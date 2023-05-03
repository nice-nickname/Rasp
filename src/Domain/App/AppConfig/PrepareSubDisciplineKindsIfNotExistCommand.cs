using System.Drawing;
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
                    Code = "л.",
                    Color = Color.Brown,
                    Type = SubDisciplineKind.OfType.LECTURE
            });
            Repository.Save(new SubDisciplineKind
            {
                    Name = "Практика",
                    Code = "пр.",
                    Color = Color.Violet,
                    Type = SubDisciplineKind.OfType.PRACTICE
            });
            Repository.Save(new SubDisciplineKind
            {
                    Name = "Лабораторная работа",
                    Code = "лаб.",
                    Color = Color.DarkKhaki,
                    Type = SubDisciplineKind.OfType.PRACTICE
            });
            Repository.Save(new SubDisciplineKind
            {
                    Name = "Экзамен",
                    Code = "экз.",
                    Color = Color.DodgerBlue,
                    Type = SubDisciplineKind.OfType.EXAM
            });
        }
    }
}
