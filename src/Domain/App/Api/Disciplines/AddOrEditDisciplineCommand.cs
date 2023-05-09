using Domain.Persistence;
using Domain.Persistence.Specification;
using FluentValidation;
using Incoding.Core.CQRS.Core;
using Resources;
using System.Numerics;

namespace Domain.Api;

public class AddOrEditDisciplineCommand : CommandBase
{
    public int? Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public int KindId { get; set; }

    public int? DepartmentId { get; set; }

    public List<int>? GroupIds { get; set; }

    public List<SubDisciplineItem>? SubDisciplines { get; set; }

    protected override void Execute()
    {
        GroupIds ??= new List<int>();
        SubDisciplines ??= new List<SubDisciplineItem>();

        var discipline = Repository.GetById<Discipline>(Id) ?? new Discipline();

        discipline.Name = Name;
        discipline.Code = Code;
        discipline.KindId = KindId;

        discipline.DepartmentId = DepartmentId;

        Repository.SaveOrUpdate(discipline);

        var disciplineGroups = Repository.Query(new DisciplineGroups.Where.ByDiscipline(discipline.Id))
                                         .Select(s => s.Id)
                                         .Cast<object>();
        if (disciplineGroups.Any())
        {
            Repository.DeleteByIds<DisciplineGroups>(disciplineGroups);
        }

        foreach (var groupId in GroupIds)
        {
            Repository.Save(new DisciplineGroups
            {
                    GroupId = groupId,
                    DisciplineId = discipline.Id
            });
        }

        foreach (var sd in SubDisciplines)
        {
            // TODO 07.05.2023: Вынести это (под-дисциплины) в под-команду
            sd.TeacherIds ??= new List<int>();
            sd.AuditoriumIds ??= new List<int>();
            sd.AuditoriumKindIds ??= new List<int>();
            sd.Plans ??= new List<PlanItem>();

            var subDisciplineItem = Repository.GetById<SubDiscipline>(sd.Id) ?? new SubDiscipline();
            subDisciplineItem.Hours = sd.Hours;
            subDisciplineItem.DisciplineId = discipline.Id;
            subDisciplineItem.KindId = sd.KindId;
            subDisciplineItem.IsParallelHours = sd.IsParallelHours;

            var sdKind = Repository.LoadById<SubDisciplineKind>(sd.KindId);
            if (sdKind.Type == SubDisciplineKind.OfType.EXAM)
            {
                subDisciplineItem.IsParallelHours = false;
            }

            Repository.SaveOrUpdate(subDisciplineItem);

        #region Сохранение преподов

            var subDisciplineTeachers = Repository.Query(new Share.Where.BySubDiscipline<SubDisciplineTeachers>(subDisciplineItem.Id))
                                                  .Select(s => s.Id)
                                                  .Cast<object>();

            if (subDisciplineTeachers.Any())
            {
                Repository.DeleteByIds<SubDisciplineTeachers>(subDisciplineTeachers);
            }

            foreach (var sdTeacherId in sd.TeacherIds)
            {
                Repository.Save(new SubDisciplineTeachers
                {
                        SubDisciplineId = subDisciplineItem.Id,
                        TeacherId = sdTeacherId
                });
            }

        #endregion

        #region Сохранение аудиторий

            var auditoriums = Repository.Query(new Share.Where.BySubDiscipline<SubDisciplineAuditoriums>(subDisciplineItem.Id))
                                        .Select(s => (object)s.Id)
                                        .ToList();
            if (auditoriums.Any())
            {
                Repository.DeleteByIds<SubDisciplineAuditoriums>(auditoriums);
            }

            foreach (var sdAuditorium in sd.AuditoriumIds)
            {
                Repository.Save(new SubDisciplineAuditoriums
                {
                        SubDisciplineId = subDisciplineItem.Id,
                        AuditoriumId = sdAuditorium
                });
            }

        #endregion

        #region Сохранение типов аудиторий

            var auditoriumsKinds = Repository.Query(new Share.Where.BySubDiscipline<SubDisciplineAuditoriumKinds>(subDisciplineItem.Id))
                                             .Select(s => (object)s.Id)
                                             .ToList();
            if (auditoriumsKinds.Any())
            {
                Repository.DeleteByIds<SubDisciplineAuditoriumKinds>(auditoriumsKinds);
            }

            foreach (var sdAuditoriumKind in sd.AuditoriumKindIds)
            {
                Repository.Save(new SubDisciplineAuditoriumKinds
                {
                        SubDisciplineId = subDisciplineItem.Id,
                        AuditoriumKindId = sdAuditoriumKind
                });
            }

        #endregion

            var plans = sd.Plans
                          .Where(s => s.Id.HasValue)
                          .Select(s => (object)s.Id.GetValueOrDefault())
                          .ToList();

            if (plans.Any())
            {
                Repository.DeleteByIds<DisciplinePlan>(plans);
            }

            if (sdKind.Type == SubDisciplineKind.OfType.EXAM)
            {
                continue;
            }

            foreach (var plan in sd.Plans)
            {
                var planItem = new DisciplinePlan
                {
                        GroupId = plan.GroupId,
                        TeacherId = plan.TeacherId,
                        SubDisciplineId = subDisciplineItem.Id,
                        SubGroupsCount = plan.SubGroupCount
                };
                Repository.Save(planItem);

                // Копируем часы по первому плану для всех оставшихся
                var actualPlan = plan;
                if (subDisciplineItem.IsParallelHours)
                {
                    actualPlan = sd.Plans.First(s => s.GroupId == plan.GroupId);
                }

                foreach (var planWeek in actualPlan.WeekItems)
                {
                    Repository.Save(new DisciplinePlanByWeek
                    {
                            DisciplinePlanId = planItem.Id,
                            AssignmentHours = planWeek.Hours,
                            Week = planWeek.Week
                    });
                }
            }
        }
    }

    public record SubDisciplineItem
    {
        public int? Id { get; set; }

        public int KindId { get; set; }

        public SubDisciplineKind.OfType Type { get; set; }

        public int Hours { get; set; }

        public string Name { get; set; }

        public bool IsParallelHours { get; set; }

        public List<int>? TeacherIds { get; set; }

        public List<int>? AuditoriumKindIds { get; set; }

        public List<int>? AuditoriumIds { get; set; }

        public List<PlanItem>? Plans { get; set; }
    }

    public record PlanItem
    {
        public int? Id { get; set; }

        public int GroupId { get; set; }

        public int TeacherId { get; set; }

        public int SubGroupCount { get; set; }

        public List<WeekItem> WeekItems { get; set; }
    }

    public record WeekItem
    {
        public int Hours { get; set; }

        public int Week { get; set; }
    }

    public class AsQuery : QueryBase<AddOrEditDisciplineCommand>
    {
        public int? Id { get; set; }

        protected override AddOrEditDisciplineCommand ExecuteResult()
        {
            var discipline = Repository.GetById<Discipline>(Id) ?? new Discipline();

            List<SubDisciplineItem> subDisciplines;

            if (Id.HasValue)
            {
                subDisciplines = discipline.SubDisciplines
                                           .ToList()
                                           .Select(s => new SubDisciplineItem
                                           {
                                                   Name = s.Kind.Name,
                                                   Hours = s.Hours,
                                                   KindId = s.KindId,
                                                   Id = s.Id,
                                                   Type = s.Kind.Type,
                                                   IsParallelHours = s.IsParallelHours,
                                                   TeacherIds = s.Teachers.Select(r => r.Id).ToList(),
                                                   AuditoriumKindIds = s.AuditoriumKinds.Select(r => r.Id).ToList(),
                                                   AuditoriumIds = s.Auditoriums.Select(r => r.Id).ToList(),
                                           })
                                           .ToList();
            }
            else
            {
                subDisciplines = Repository.Query<SubDisciplineKind>()
                                           .Select(s => new SubDisciplineItem
                                           {
                                                   KindId = Convert.ToInt32(s.Id),
                                                   Hours = 0,
                                                   Name = s.Name,
                                                   TeacherIds = new List<int>(),
                                                   AuditoriumKindIds = new List<int>(),
                                                   AuditoriumIds = new List<int>(),
                                                   Type = s.Type,
                                                   IsParallelHours = false
                                           })
                                           .ToList();
            }

            return new AddOrEditDisciplineCommand
            {
                    Id = Id,
                    Name = discipline.Name,
                    Code = discipline.Code,
                    DepartmentId = discipline.DepartmentId,
                    KindId = discipline.KindId,
                    SubDisciplines = subDisciplines,
                    GroupIds = discipline.Groups.Select(s => s.Id).ToList()
            };
        }
    }

    public class Validator : AbstractValidator<AddOrEditDisciplineCommand>
    {
        public Validator()
        {
            RuleFor(s => s.Name).NotEmpty().NotNull().WithName(DataResources.DisciplineName);
            RuleFor(s => s.Code).NotEmpty().NotNull().WithName(DataResources.Abbreviation);
            RuleFor(s => s.KindId).NotEmpty().NotNull().WithName(DataResources.DisciplineType);
            RuleFor(s => s.GroupIds).NotEmpty().NotNull().WithName(DataResources.GroupList);
        }
    }
}
