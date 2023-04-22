using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class FacultySettings : IncEntityBase
{
    public enum OfType
    {
        StartDate,
        CountOfWeeks
    }

    public virtual int Id { get; set; }

    public virtual string Value { get; set; }

    public virtual OfType Type { get; set; }

    public virtual int FacultyId { get; set; }

    public class Mapping : ClassMap<FacultySettings>
    {
        public Mapping()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Type).CustomType<OfType>();
            Map(s => s.Value);
            Map(s => s.FacultyId);
        }
    }

    public class Where
    {
    }
}
