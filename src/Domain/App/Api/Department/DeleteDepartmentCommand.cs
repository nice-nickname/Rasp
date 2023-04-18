using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class DeleteDepartmentCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
            Repository.Delete<Department>(Id);
    }
}
