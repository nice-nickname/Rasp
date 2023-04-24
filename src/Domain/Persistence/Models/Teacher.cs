using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class Teacher : IncEntityBase, Share.IEntityHasDepartment
{
    public new virtual int Id { get; set; }

    public virtual string Name { get; set; }

    public virtual int DepartmentId { get; set; }

    public virtual Department Department { get; set; }

    public class Mapping : ClassMap<Teacher>
    {
        public Mapping()
        {
            Table(nameof(Teacher));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Name);
            Map(s => s.DepartmentId);

            References(s => s.Department).Column(nameof(DepartmentId))
                                         .ReadOnly()
                                         .LazyLoad();
        }
    }
}
