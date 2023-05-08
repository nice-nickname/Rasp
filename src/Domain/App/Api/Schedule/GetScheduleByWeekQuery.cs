using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;
using Resources;

namespace Domain.Api.Schedule;

public class GetScheduleByWeekQuery : QueryBase<List<GetScheduleByWeekQuery.Response>>
{
    public int Week { get; set; }

    public int? GroupId { get; set; }

    public int? AuditoriumId { get; set; }

    public int? TeacherId { get; set; }

    public int FacultyId { get; set; }

    protected override List<Response> ExecuteResult()
    {
        var schedulerItems = Repository.Query<ScheduleFormat>()
                                       .Where(s => s.FacultyId == FacultyId)
                                       .OrderBy(s => s.Order)
                                       .Select(s => new AddOrEditScheduleFormatCommand.ScheduleItem
                                       {
                                               Start = s.Start,
                                               End = s.End,
                                               Order = s.Order,
                                               Id = s.Id
                                       })
                                       .ToList();

        var classes = new List<Response>
        {
                new() { Day = DayOfWeek.Monday, DayString = DataResources.Monday },
                new() { Day = DayOfWeek.Tuesday, DayString = DataResources.Tuesday },
                new() { Day = DayOfWeek.Wednesday, DayString = DataResources.Wednesday },
                new() { Day = DayOfWeek.Thursday, DayString = DataResources.Thursday },
                new() { Day = DayOfWeek.Friday, DayString = DataResources.Friday },
                new() { Day = DayOfWeek.Saturday, DayString = DataResources.Saturday }
        };
        foreach (var @class in classes)
        {
            @class.Items = new List<ClassItem>();
            for (var i = 0; i < schedulerItems.Count; i++)
                @class.Items.Add(new ClassItem
                {
                        Order = i,
                        IsEmpty = true,
                        ScheduleFormatId = schedulerItems[i].Id
                });
        }

        var scheduledClasses = Repository.Query<Class>()
                                         .Where(r => r.Plan.GroupId == GroupId
                                                  && r.Week == Week)
                                         .Select(r => new ClassItem
                                         {
                                                 Color = r.Plan.SubDiscipline.Kind.Color.ToHex(),
                                                 DisciplinePlanId = r.DisciplinePlanId,
                                                 TeacherId = r.Plan.TeacherId,
                                                 Teacher = r.Plan.Teacher.ShortName,
                                                 Department = r.Plan.SubDiscipline.Discipline.Department.Name,
                                                 DepartmentCode = r.Plan.SubDiscipline.Discipline.Department.Code,
                                                 DisciplineId = r.Plan.SubDiscipline.DisciplineId,
                                                 Discipline = r.Plan.SubDiscipline.Discipline.Name,
                                                 DisciplineCode = r.Plan.SubDiscipline.Discipline.Code,
                                                 SubDisciplineCode = r.Plan.SubDiscipline.Kind.Code,
                                                 SubDiscipline = r.Plan.SubDiscipline.Kind.Name,
                                                 SubDisciplineId = r.Plan.SubDiscipline.Id,
                                                 SubGroupNo = r.SubGroupNo,
                                                 HasSubGroups = r.SubGroupNo > 0,
                                                 GroupId = r.Plan.GroupId,
                                                 Group = r.Plan.Group.Code,
                                                 Day = r.Day,
                                                 Order = r.ScheduleFormat.Order,
                                                 IsEmpty = false,
                                                 ScheduleFormatId = r.ScheduleFormatId,
                                                 Id = r.Id,
                                                 AuditoriumId = r.AuditoriumId,
                                                 Auditorium = r.Auditorium != null ? $"{r.Auditorium.Building.Name}-{r.Auditorium.Code}" : DataResources.ChooseAuditorium
                                         })
                                         .ToList()
                                         .GroupBy(r => r.Day)
                                         .ToList();

        foreach (var scheduled in scheduledClasses)
        {
            foreach (var item in scheduled)
            {
                if (!classes.Any(r => r.Day == scheduled.Key && r.Items.Any(q => item.Order == q.Order)))
                    continue;
                {
                    classes.First(r => r.Day == scheduled.Key).Items.Remove(classes.First(r => r.Day == scheduled.Key).Items.First(r => item.Order == r.Order));
                    classes.First(r => r.Day == scheduled.Key).Items.Add(item);
                }
            }
        }

        foreach (var @class in classes)
        {
            @class.Items = @class.Items.OrderBy(r => r.Order).ToList();
        }

        classes = classes.OrderBy(r => r.Day).ToList();

        return classes;
    }

    public class Response
    {
        public string DayString { get; set; }

        public DayOfWeek Day { get; set; }

        public List<ClassItem> Items { get; set; }
    }

    public class ClassItem
    {
        public int Id { get; set; }

        public int ScheduleFormatId { get; set; }

        public int Order { get; set; }

        public int GroupId { get; set; }

        public int DisciplineId { get; set; }

        public int SubDisciplineId { get; set; }

        public int TeacherId { get; set; }

        public int SubGroupNo { get; set; }

        public int DisciplinePlanId { get; set; }

        public int? AuditoriumId { get; set; }

        public string Group { get; set; }

        public string Discipline { get; set; }

        public string DisciplineCode { get; set; }

        public string SubDiscipline { get; set; }

        public string Teacher { get; set; }

        public string Department { get; set; }

        public string DepartmentCode { get; set; }

        public string Color { get; set; }

        public string SubDisciplineCode { get; set; }

        public string Auditorium { get; set; }

        public bool HasSubGroups { get; set; }

        public bool IsEmpty { get; set; }

        public DayOfWeek Day { get; set; }
    }
}
