using System.Linq.Expressions;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;
using Incoding.Core.Extensions.LinqSpecs;

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
        public class ByFacultyAndType : Specification<FacultySettings>
        {
            private readonly int _facultyId;

            private readonly OfType _type;

            public ByFacultyAndType(int facultyId, OfType type)
            {
                this._type = type;
                this._facultyId = facultyId;
            }

            public override Expression<Func<FacultySettings, bool>> IsSatisfiedBy()
            {
                return s => s.FacultyId == this._facultyId && s.Type == this._type;
            }
        }
    }
}
