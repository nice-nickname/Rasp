using Domain.App.Api;
using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;
using Resources;

namespace Domain.Api;

public class GetScheduleByWeekQuery : QueryBase<List<GetScheduleByWeekQuery.Response>>
{
    private readonly Func<DateTime, DayOfWeek, DateTime> getDay = (startDate, day) =>
    {
        int diff = day - startDate.DayOfWeek;
        return startDate.AddDays(diff).Date;
    };

    public int Week { get; set; }

    public int? SelectedGroupId { get; set; }

    public int? SelectedAuditoriumId { get; set; }

    public int? SelectedTeacherId { get; set; }

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

        var startWeekDate = Dispatcher.Query(new GetDateFromWeekQuery { FacultyId = FacultyId, Week = Week });
        var weekends = Dispatcher.Query(new GetWeekendsForWeekQuery { StartDate = startWeekDate });
        var scheduleFormat = Dispatcher.Query(new GetScheduleFormatQuery { FacultyId = FacultyId });

        var classes = new List<Response>
        {
                new() { Day = DayOfWeek.Monday, DayString = DataResources.Monday, Date = startWeekDate.ToShortDateString() },
                new() { Day = DayOfWeek.Tuesday, DayString = DataResources.Tuesday, Date = startWeekDate.AddDays(1).ToShortDateString() },
                new() { Day = DayOfWeek.Wednesday, DayString = DataResources.Wednesday, Date = startWeekDate.AddDays(2).ToShortDateString() },
                new() { Day = DayOfWeek.Thursday, DayString = DataResources.Thursday, Date = startWeekDate.AddDays(3).ToShortDateString() },
                new() { Day = DayOfWeek.Friday, DayString = DataResources.Friday, Date = startWeekDate.AddDays(4).ToShortDateString() },
                new() { Day = DayOfWeek.Saturday, DayString = DataResources.Saturday, Date = startWeekDate.AddDays(5).ToShortDateString() }
        };
        foreach (var @class in classes)
        {
            @class.Items = new List<ClassItem>();
            for (var i = 0; i < schedulerItems.Count; i++)
            {
                var currentDate = this.getDay(startWeekDate, @class.Day);
                var isBlocked = weekends.Contains(DateOnly.FromDateTime(currentDate))
                             || currentDate < scheduleFormat.StartDate
                             || currentDate > scheduleFormat.EndDate;

                @class.Items.Add(new ClassItem
                {
                        Order = i,
                        IsEmpty = true,
                        ScheduleFormatId = schedulerItems[i].Id.GetValueOrDefault(),
                        IsBlocked = isBlocked
                });
            }
        }

        var scheduledClassesAll = Repository.Query<Class>();

        if (SelectedGroupId.HasValue)
            scheduledClassesAll = scheduledClassesAll.Where(r => r.Plan.GroupId == SelectedGroupId.Value && r.Week == Week);

        if (SelectedAuditoriumId.HasValue)
            scheduledClassesAll = scheduledClassesAll.Where(r => r.AuditoriumId == SelectedAuditoriumId && r.Week == Week);

        if (SelectedTeacherId.HasValue)
            scheduledClassesAll = scheduledClassesAll.Where(r => r.Plan.TeacherId == SelectedTeacherId && r.Week == Week);

        var scheduledClasses = scheduledClassesAll
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
                                       Auditorium = r.Auditorium != null ? $"{r.Auditorium.Building.Name}-{r.Auditorium.Code}" : DataResources.ChooseAuditorium,
                                       IsGroup = SelectedGroupId.HasValue,
                                       IsAuditorium = SelectedAuditoriumId.HasValue,
                                       IsTeacher = SelectedTeacherId.HasValue,
                                       StudentCount = r.Plan.Group.StudentCount
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

        public string Date { get; set; }

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

        public int StudentCount { get; set; }

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

        public bool IsGroup { get; set; }

        public bool IsAuditorium { get; set; }

        public bool IsTeacher { get; set; }

        public bool IsBlocked { get; set; }

        public DayOfWeek Day { get; set; }
    }
}
