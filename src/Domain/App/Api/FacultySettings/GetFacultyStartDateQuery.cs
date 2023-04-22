using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class AddOrEditFacultySettingCommand<T> : CommandBase
{
    public int FacultyId { get; set; }

    public FacultySettings.OfType Type { get; set; }

    public T Value { get; set; }

    protected override void Execute()
    {
        // m-debug убрать повторяющийся спек в FacultySettings.Where
        var setting = Repository.Query<FacultySettings>()
                                .Where(s => s.FacultyId == FacultyId &&
                                            s.Type == Type)
                                .FirstOrDefault() ?? new FacultySettings { FacultyId = FacultyId, Type = Type };

        setting.Value = Value.ToString();
        Repository.SaveOrUpdate(setting);
    }
}

public class GetFacultyCountOfWeeksCommand : QueryBase<int>
{
    public int FacultyId { get; set; }

    protected override int ExecuteResult()
    {
        var weeksSetting = Repository.Query<FacultySettings>()
                                     .Where(s => s.Type == FacultySettings.OfType.CountOfWeeks &&
                                                 s.FacultyId == FacultyId)
                                     .FirstOrDefault();

        if (weeksSetting == null)
        {
            var weeks = 17;
            weeksSetting = new FacultySettings
            {
                FacultyId = FacultyId,
                Value = weeks.ToString(),
                Type = FacultySettings.OfType.CountOfWeeks
            };
            Repository.Save(weeksSetting);
        }
        return int.Parse(weeksSetting.Value);
    }
}

public class GetFacultyStartDateCommand : QueryBase<DateTime>
{
    public int FacultyId { get; set; }

    protected override DateTime ExecuteResult()
    {
        var dateSetting = Repository.Query<FacultySettings>()
                                    .Where(s => s.Type == FacultySettings.OfType.StartDate &&
                                                s.FacultyId == FacultyId)
                                    .FirstOrDefault();

        if (dateSetting == null)
        {
            var now = new DateTime(DateTime.Now.Year, 9, 1);
            dateSetting = new FacultySettings
            {
                FacultyId = FacultyId,
                Value = now.ToString(),
                Type = FacultySettings.OfType.StartDate
            };
            Repository.Save(dateSetting);
        }
        return DateTime.Parse(dateSetting.Value);

    }
}
