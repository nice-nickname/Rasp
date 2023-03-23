using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class ScheduleTable : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual DateTime StartTime { get; set; }

    public virtual DateTime EndTime { get; set; }

    internal class Map : ClassMap<ScheduleTable>
    {
        public Map()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.StartTime);
            Map(s => s.EndTime);
        }
    }
}