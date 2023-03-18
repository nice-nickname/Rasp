using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class Class : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual string Name { get; set; }

    public virtual IList<ClassModule>? Modules { get; set; }

    internal class Map : ClassMap<Class>
    {
        public Map()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Name);
            HasMany(s => s.Modules).KeyColumn(nameof(ClassModule.ClassId))
                                   .ReadOnly()
                                   .LazyLoad();
        }
    }

    public enum OfType
    {
        Lecture,
        Practice,
        Laboratory,
        Consultation,
        Exam
    }
}
