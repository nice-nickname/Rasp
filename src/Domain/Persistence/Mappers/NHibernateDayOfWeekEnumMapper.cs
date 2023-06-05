using System.Diagnostics.CodeAnalysis;

namespace Domain.Persistence.Mappers;

[ExcludeFromCodeCoverage]
public class NHibernateDayOfWeekEnumMapper : global::NHibernate.Type.PersistentEnumType
{
    public NHibernateDayOfWeekEnumMapper()
            : base(typeof(DayOfWeek))
    {

    }
}
