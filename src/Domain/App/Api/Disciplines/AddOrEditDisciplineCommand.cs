using Incoding.Core.CQRS.Core;
using Domain.Persistence;

namespace Domain.Api;

public class AddOrEditDisciplineCommand : CommandBase
{
    public int? Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public Discipline.OfType Type { get; set; }

    public int DepartmentId { get; set; }

    //public int[] GroupIds { get; set; }

    //public int[] TeacherIds { get; set; }

    public List<SubDisciplineItem> SubDisciplines { get; set; }

    protected override void Execute()
    {
        var discipline = Repository.GetById<Discipline>(Id) ?? new Discipline();

        discipline.Name = Name;
        discipline.Code = Code;
        discipline.DepartmentId = DepartmentId;
        discipline.Type = Type;

        Repository.SaveOrUpdate(discipline);

        //Repository.DeleteByIds<DisciplineGroups>(Repository.Query<DisciplineGroups>()
        //                                                   .Where(s => s.DisciplineId == discipline.Id)
        //                                                   .Select(s => s.Id)
        //                                                   .Cast<object>()
        //                                                   .ToList());

        //foreach (var groupId in GroupIds)
        //{
        //    Repository.Save(new DisciplineGroups
        //    {
        //            DisciplineId = discipline.Id,
        //            GroupId = groupId,
        //    });
        //}

        //Repository.DeleteByIds<DisciplineTeachers>(Repository.Query<DisciplineTeachers>()
        //                                                     .Where(s => s.DisciplineId == discipline.Id)
        //                                                     .Select(s => s.Id)
        //                                                     .Cast<object>()
        //                                                     .ToList());

        //foreach (var teacherId in TeacherIds)
        //{
        //    Repository.Save(new DisciplineTeachers
        //    {
        //            DisciplineId = discipline.Id,
        //            TeacherId = teacherId,
        //    });
        //}

        foreach (var subDisciplines in SubDisciplines)
        {
            var subDiscipline = Repository.GetById<SubDiscipline>(subDisciplines.Id) ?? new SubDiscipline();
            subDiscipline.DisciplineId = discipline.Id;
            subDiscipline.Hours = subDisciplines.Hours;
            subDiscipline.Type = subDisciplines.Type;
            Repository.SaveOrUpdate(subDiscipline);
        }
    }

    public class SubDisciplineItem
    {
        public int? Id { get; set; }

        public int Hours { get; set; }

        public SubDiscipline.OfType Type { get; set; }
    }

    public class AsQuery : QueryBase<AddOrEditDisciplineCommand>
    {
        protected override AddOrEditDisciplineCommand ExecuteResult()
        {
            return new AddOrEditDisciplineCommand
            {
                    SubDisciplines = new List<SubDisciplineItem>
                    {
                            new() { Type = SubDiscipline.OfType.LECTURE, Hours = 0 },
                            new() { Type = SubDiscipline.OfType.PRACTICE, Hours = 0 },
                            new() { Type = SubDiscipline.OfType.LAB, Hours = 0 }
                    }
            };
        }
    }
}
