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
        var setting = Repository.Query(new FacultySettings.Where.ByFacultyAndType(FacultyId, Type))
                                .FirstOrDefault() ?? new FacultySettings { FacultyId = FacultyId, Type = Type };

        setting.Value = Value.ToString();

        Repository.SaveOrUpdate(setting);
    }

    public class IfNotExist : CommandBase
    {
        public int FacultyId { get; set; }

        public FacultySettings.OfType Type { get; set; }

        public T Value { get; set; }

        protected override void Execute()
        {
            var setting = Repository.Query(new FacultySettings.Where.ByFacultyAndType(FacultyId, Type))
                                    .FirstOrDefault();

            if (setting == null)
            {
                Repository.Save(new FacultySettings
                {
                        FacultyId = FacultyId,
                        Type = Type,
                        Value = Value.ToString()
                });
            }
        }
    }
}
