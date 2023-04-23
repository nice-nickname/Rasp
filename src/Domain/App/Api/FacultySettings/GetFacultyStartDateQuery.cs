using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetFacultySettingCommand<T> : QueryBase<T> where T : struct
{
    public int FacultyId { get; set; }

    public FacultySettings.OfType Type { get; set; }

    protected override T ExecuteResult()
    {
        // TODO 23.04.2023: Возможно возвращать тут nullable, а не default(T)
        return (T)Convert.ChangeType(Repository.Query(new FacultySettings.Where.ByFacultyAndType(FacultyId, Type))
                                               .First().Value, typeof(T));
    }
}
