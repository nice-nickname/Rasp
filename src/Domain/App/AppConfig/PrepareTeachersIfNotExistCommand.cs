namespace Domain.Api;

using Domain.Persistence;
using Incoding.Core.CQRS.Core;

public class PrepareTeachersIfNotExistCommand : CommandBase
{
    protected override void Execute()
    {
        var departmentId = Repository.Query<Department>()
                                     .Select(s => s.Id)
                                     .FirstOrDefault();

        if (!Repository.Query<Teacher>().Any())
        {
            Repository.Save(new Teacher
            {
                    DepartmentId = departmentId,
                    Name = "Хусаинов Наиль Шавкятович"
            });
            Repository.Save(new Teacher
            {
                    DepartmentId = departmentId,
                    Name = "Альминене Татьяна Александровна"
            });
            Repository.Save(new Teacher
            {
                    DepartmentId = departmentId,
                    Name = "Дмитриев Дмитро Дмитриевич"
            });
        }

    }
}
