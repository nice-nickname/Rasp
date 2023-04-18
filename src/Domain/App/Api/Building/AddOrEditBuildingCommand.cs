using Incoding.Core.CQRS.Core;

namespace Domain.Api.Building;

public class AddOrEditBuildingCommand : CommandBase
{
    public int? Id { get; set; }

    public string Name { get; set; }

    protected override void Execute()
    {
        var building = Repository.GetById<Persistence.Building>(Id.GetValueOrDefault()) ?? new Persistence.Building();
        building.Name = Name;

        Repository.SaveOrUpdate(building);
    }

    public class AsView : QueryBase<AddOrEditBuildingCommand>
    {
        public int? Id { get; set; }

        protected override AddOrEditBuildingCommand ExecuteResult()
        {
            var building = Repository.GetById<Persistence.Building>(Id.GetValueOrDefault()) ?? new Persistence.Building();

            return new AddOrEditBuildingCommand
            {
                    Id = building.Id,
                    Name = building.Name
            };
        }
    }
}
