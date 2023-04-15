namespace Domain.Api;

using Domain.Persistence;
using Incoding.Core.CQRS.Core;

public class PrepareGroupsIfNotExistCommand : CommandBase
{
    protected override void Execute()
    {
        var departments = Repository.Query<Department>()
                                    .Select(s => s.Id)
                                    .ToList();

        foreach (var department in departments)
        {
            var groupCount = Repository.Query<Group>()
                                       .Count(s => s.DepartmentId == department);

            if (groupCount == 0)
            {
                Repository.Save(new Group
                {
                        Code = "КТбо4-5",
                        DepartmentId = department
                });
                Repository.Save(new Group
                {
                        Code = "КТбо4-6",
                        DepartmentId = department
                });
                Repository.Save(new Group
                {
                        Code = "КТбо4-7",
                        DepartmentId = department
                });
                Repository.Save(new Group
                {
                        Code = "КТбо4-8",
                        DepartmentId = department
                });
            }
        }
    }
}
