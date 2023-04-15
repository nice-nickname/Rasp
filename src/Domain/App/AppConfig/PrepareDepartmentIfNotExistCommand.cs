namespace Domain.Api;

using Domain.Persistence;
using Incoding.Core.CQRS.Core;

public class PrepareDepartmentIfNotExistCommand : CommandBase
{
    protected override void Execute()
    {
        var faculties = Repository.Query<Faculty>()
                                  .Select(s => s.Id)
                                  .ToList();

        foreach (var faculty in faculties)
        {
            var departmentCount = Repository.Query<Department>()
                                            .Count(s => s.FacultyId == faculty);

            if (departmentCount == 0)
            {
                Repository.Save(new Department
                {
                        Name = "Математического обеспечения и применения ЭВМ",
                        Code = "МОП ЭВМ",
                        FacultyId = faculty
                });
                Repository.Save(new Department
                {
                        Name = "Системного анализа и телекоммуникаций",
                        Code = "САИТ",
                        FacultyId = faculty
                });
            }
        }
    }
}
