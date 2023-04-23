using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class AddOrEditGroupCommand : CommandBase
{
    public int? Id { get; set; }

    public int DepartmentId { get; set; }

    public int StudentCount { get; set; }

    public string Code { get; set; }

    protected override void Execute()
    {
        var group = Repository.GetById<Group>(Id) ?? new Group();

        group.Code = Code;
        group.DepartmentId = DepartmentId;
        group.StudentCount = StudentCount;

        Repository.SaveOrUpdate(group);
    }

    public class AsView : QueryBase<AddOrEditGroupCommand>
    {
        public int? Id { get; set; }

        protected override AddOrEditGroupCommand ExecuteResult()
        {
            var group = Repository.GetById<Group>(Id) ?? new Group();

            return new AddOrEditGroupCommand
            {
                    Id = group.Id,
                    Code = group.Code,
                    DepartmentId = group.DepartmentId,
                    StudentCount = group.StudentCount
            };
        }
    }
}
