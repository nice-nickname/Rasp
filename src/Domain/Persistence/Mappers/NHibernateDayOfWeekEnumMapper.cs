namespace Domain.Persistence.Mappers;

public class NHibernateDayOfWeekEnumMapper : global::NHibernate.Type.PersistentEnumType
{
    public NHibernateDayOfWeekEnumMapper()
            : base(typeof(DayOfWeek))
    {

    }
}