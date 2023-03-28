using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class Teacher : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual string Name { get; set; }

    public virtual int DepartmentId { get; set; }

    public virtual Department Department { get; set; }

    public class Mapping : ClassMap<Teacher>
    {
        public Mapping()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Name);
            Map(s => s.DepartmentId);

            References(s => s.Department).Column(nameof(DepartmentId))
                                         .ReadOnly()
                                         .LazyLoad();
        }
    }
}