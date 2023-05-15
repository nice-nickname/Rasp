using Domain.Persistence;
using FluentValidation;
using Incoding.Core.CQRS.Core;
using Resources;

namespace Domain.Api;

public class AddOrEditBuildingCommand : CommandBase
{
    public int? Id { get; set; }

    public string Name { get; set; }

    protected override void Execute()
    {
        var building = Repository.GetById<Building>(Id.GetValueOrDefault()) ?? new Building();
        building.Name = Name;

        Repository.SaveOrUpdate(building);
    }

    public class AsQuery : QueryBase<AddOrEditBuildingCommand>
    {
        public int? Id { get; set; }

        protected override AddOrEditBuildingCommand ExecuteResult()
        {
            var building = Repository.GetById<Building>(Id.GetValueOrDefault()) ?? new Building();

            return new AddOrEditBuildingCommand
            {
                    Id = building.Id,
                    Name = building.Name
            };
        }
    }

    public class Validator : AbstractValidator<AddOrEditBuildingCommand>
    {
        public Validator()
        {
            RuleFor(b => b.Name).NotEmpty().WithMessage(DataResources.InvalidEmpty);
        }
    }
}
