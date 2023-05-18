using Domain.App.Api;
using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;
using Resources;

namespace Domain.Api;

public class GetScheduleByWeekQuery : QueryBase<List<GetScheduleByWeekQuery.Response>>
{
    public enum ModeOf
    {
        Groups,

        Auditoriums,

        Teachers
    }

    private readonly Func<DateTime, DayOfWeek, DateTime> getDay = (startDate, day) =>
    {
        int diff = day - startDate.DayOfWeek;
        return startDate.AddDays(diff).Date;
    };

    public int Week { get; set; }

    public int FacultyId { get; set; }

    public int?[] SelectedGroupIds { get; set; }

    public int?[] SelectedAuditoriumIds { get; set; }

    public int?[] SelectedTeacherIds { get; set; }

    public DayOfWeek? Day { get; set; }

    protected override List<Response> ExecuteResult()
    {
        if (SelectedGroupIds.Length <= 1 && SelectedAuditoriumIds.Length <= 1 && SelectedTeacherIds.Length <= 1)
        {
            var selectedGroupId = SelectedGroupIds.FirstOrDefault();
            var selectedAuditoriumId = SelectedAuditoriumIds.FirstOrDefault();
            var selectedTeacherId = SelectedTeacherIds.FirstOrDefault();
            var mode = selectedGroupId != null
                    ? ModeOf.Groups
                    : selectedAuditoriumId != null
                            ? ModeOf.Auditoriums
                            : ModeOf.Teachers;

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
            var weekends = Dispatcher.Query(new GetWeekendsForWeekQuery
            {
                    FacultyId = FacultyId,
                    StartDate = startWeekDate
            });

            var preferences = new List<GetTeacherPreferencesQuery.Response>();
            var scheduledClassesAll = Repository.Query<Class>();

            switch (mode)
            {
                case ModeOf.Groups:
                    scheduledClassesAll = scheduledClassesAll.Where(r => r.Plan.GroupId == selectedGroupId.Value && r.Week == Week);

                    break;

                case ModeOf.Auditoriums:
                    scheduledClassesAll = scheduledClassesAll.Where(r => r.AuditoriumId == selectedAuditoriumId && r.Week == Week);

                    break;

                case ModeOf.Teachers:
                    scheduledClassesAll = scheduledClassesAll.Where(r => r.Plan.TeacherId == selectedTeacherId && r.Week == Week);
                    preferences = Dispatcher.Query(new GetTeacherPreferencesQuery
                    {
                            FacultyId = FacultyId,
                            TeacherId = selectedTeacherId
                    });

                    break;
            }

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
                    var isBlocked = weekends.Contains(DateOnly.FromDateTime(currentDate));
                    var isUnwanted = false;
                    if (mode is ModeOf.Teachers && preferences is { Count: > 0 })
                    {
                        var currentPreference = preferences.First(r => r.Day == @class.Day).Days.First(r => r.ScheduleItemId == schedulerItems[i].Id.GetValueOrDefault());
                        isBlocked = isBlocked || currentPreference.Type == GetTeacherPreferencesQuery.PreferenceType.IMPOSSIBLE;
                        isUnwanted = currentPreference.Type == GetTeacherPreferencesQuery.PreferenceType.UNWANTED;
                    }

                    @class.Items.Add(new ClassItem
                    {
                            Order = i,
                            IsEmpty = true,
                            ScheduleFormatId = schedulerItems[i].Id.GetValueOrDefault(),
                            IsBlocked = isBlocked,
                            IsUnwanted = isUnwanted
                    });
                }
            }

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
                                           IsGroup = mode == ModeOf.Groups,
                                           IsAuditorium = mode == ModeOf.Auditoriums,
                                           IsTeacher = mode == ModeOf.Teachers,
                                           StudentCount = r.Plan.Group.StudentCount,
                                           IsUnwanted = r.IsUnwanted
                                   })
                                   .ToList()
                                   .GroupBy(r => r.Day)
                                   .ToList();

            foreach (var scheduled in scheduledClasses)
            {
                var addedMany = 0;

                foreach (var item in scheduled)
                {
                    if ((!classes.Any(r => r.Day == scheduled.Key && r.Items.Any(q => item.Order == q.Order))) || addedMany > 0)
                        continue;
                    {
                        var day = classes.First(r => r.Day == scheduled.Key);
                        var @class = classes.First(r => r.Day == scheduled.Key).Items.First(r => item.Order == r.Order);

                        if (mode is ModeOf.Teachers or ModeOf.Auditoriums)
                        {
                            var allClassesLikeCurrent = scheduled.Where(r => r.Order == item.Order).ToList();

                            if (allClassesLikeCurrent.Count > 1)
                                item.Group = string.Join(", ", allClassesLikeCurrent.Select(r => r.Group));

                            addedMany++;
                        }

                        day.Items.Remove(@class);
                        day.Items.Add(item);
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
        else
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
            var weekends = Dispatcher.Query(new GetWeekendsForWeekQuery
            {
                    FacultyId = FacultyId,
                    StartDate = startWeekDate
            });

            var preferences = new List<TeacherPreferenceItem>();
            var items = new List<int?>();
            ModeOf? mode = null;

            if (SelectedGroupIds.First() != null && SelectedGroupIds.Length > 0)
            {
                items = SelectedGroupIds.ToList();
                mode = ModeOf.Groups;
            }

            if (SelectedAuditoriumIds.First() != null && SelectedAuditoriumIds.Length > 0)
            {
                items = SelectedAuditoriumIds.ToList();
                mode = ModeOf.Auditoriums;
            }

            if (SelectedTeacherIds.First() != null && SelectedTeacherIds.Length > 0)
            {
                items = SelectedTeacherIds.ToList();
                mode = ModeOf.Teachers;
                foreach (var teacherId in items)
                    preferences.Add(new TeacherPreferenceItem
                    {
                            TeacherId = teacherId,
                            Items = Dispatcher.Query(new GetTeacherPreferencesQuery
                            {
                                    FacultyId = FacultyId,
                                    TeacherId = teacherId
                            })
                    });
            }

            var classes = new List<Response>();
            for (int i = 0; i < items.Count; i++)
                classes.Add(new Response
                {
                        Id = items[i],
                        Day = Day!.Value
                });

            foreach (var @class in classes)
            {
                switch (mode)
                {
                    case ModeOf.Groups:
                        @class.DayString = Repository.GetById<Group>(@class.Id).Code;

                        break;

                    case ModeOf.Auditoriums:
                        var auditorium = Repository.GetById<Auditorium>(@class.Id);
                        @class.DayString = $"{auditorium.Building.Name}-{auditorium.Code}";

                        break;

                    case ModeOf.Teachers:
                        @class.DayString = Repository.GetById<Teacher>(@class.Id).ShortName;

                        break;
                }

                @class.Items = new List<ClassItem>();
                for (var i = 0; i < schedulerItems.Count; i++)
                {
                    var currentDate = this.getDay(startWeekDate, Day!.Value);
                    var isBlocked = weekends.Contains(DateOnly.FromDateTime(currentDate));
                    var isUnwanted = false;
                    if (preferences != null && preferences.Count > 0)
                    {
                        var currentPreference = preferences.First(r => r.TeacherId == @class.Id).Items.First(r => r.Day == @class.Day).Days.First(r => r.ScheduleItemId == schedulerItems[i].Id.GetValueOrDefault());
                        isBlocked = isBlocked || currentPreference.Type == GetTeacherPreferencesQuery.PreferenceType.IMPOSSIBLE;
                        isUnwanted = currentPreference.Type == GetTeacherPreferencesQuery.PreferenceType.UNWANTED;
                    }

                    @class.Items.Add(new ClassItem
                    {
                            Order = i,
                            IsEmpty = true,
                            ScheduleFormatId = schedulerItems[i].Id.GetValueOrDefault(),
                            IsBlocked = isBlocked,
                            IsUnwanted = isUnwanted,
                            IsAuditorium = mode == ModeOf.Auditoriums,
                            AuditoriumId = mode == ModeOf.Auditoriums
                                    ? @class.Id
                                    : null
                    });
                }
            }

            var scheduledClassesAll = Repository.Query<Class>();
            switch (mode)
            {
                case ModeOf.Groups:
                    scheduledClassesAll = scheduledClassesAll.Where(r => SelectedGroupIds.Contains(r.Plan.GroupId)
                                                                      && r.Week == Week
                                                                      && r.Day == Day);

                    break;

                case ModeOf.Auditoriums:
                    scheduledClassesAll = scheduledClassesAll.Where(r => SelectedAuditoriumIds.Contains(r.AuditoriumId)
                                                                      && r.Week == Week
                                                                      && r.Day == Day);

                    break;

                case ModeOf.Teachers:
                    scheduledClassesAll = scheduledClassesAll.Where(r => SelectedTeacherIds.Contains(r.Plan.TeacherId)
                                                                      && r.Week == Week
                                                                      && r.Day == Day);

                    break;
            }

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
                                           IsGroup = mode == ModeOf.Groups,
                                           IsAuditorium = mode == ModeOf.Auditoriums,
                                           IsTeacher = mode == ModeOf.Teachers,
                                           StudentCount = r.Plan.Group.StudentCount,
                                           IsUnwanted = r.IsUnwanted
                                   })
                                   .ToList()
                                   .GroupBy(r => mode switch
                                   {
                                           ModeOf.Groups => r.GroupId,
                                           ModeOf.Auditoriums => r.AuditoriumId,
                                           ModeOf.Teachers => r.TeacherId,
                                           _ => throw new ArgumentOutOfRangeException()
                                   })
                                   .ToList();

            foreach (var scheduled in scheduledClasses)
            {
                foreach (var item in scheduled)
                {
                    if (!classes.Any(r => r.Id == scheduled.Key && r.Items.Any(q => item.Order == q.Order)))
                        continue;
                    {
                        classes.First(r => r.Id == scheduled.Key).Items.Remove(classes.First(r => r.Id == scheduled.Key).Items.First(r => item.Order == r.Order));
                        classes.First(r => r.Id == scheduled.Key).Items.Add(item);
                    }
                }
            }

            foreach (var @class in classes)
            {
                @class.Items = @class.Items.OrderBy(r => r.Order).ToList();
            }

            classes = classes.OrderBy(r => r.DayString).ToList();

            return classes;
        }

        return null;
    }

    public class Response
    {
        public string DayString { get; set; }

        public string Date { get; set; }

        public DayOfWeek Day { get; set; }

        public List<ClassItem> Items { get; set; }

        public int? Id { get; set; }
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

        public bool IsUnwanted { get; set; }

        public DayOfWeek Day { get; set; }
    }

    private class TeacherPreferenceItem
    {
        public int? TeacherId { get; set; }

        public List<GetTeacherPreferencesQuery.Response> Items { get; set; }
    }
}
