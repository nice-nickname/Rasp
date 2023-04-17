using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class AddOrEditDisciplineCommand : CommandBase
{
    protected override void Execute()
    {
        throw new NotImplementedException();
    }

    public class AsQuery : QueryBase<AddOrEditDisciplineCommand>
    {
        protected override AddOrEditDisciplineCommand ExecuteResult()
        {
            return new AddOrEditDisciplineCommand();
        }
    }
}
