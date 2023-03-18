using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class Teacher : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual string Name { get; set; }

    public virtual int DepartmentId { get; set; }

    public virtual Department Department { get; set; }

    internal class Map : ClassMap<Teacher>
    {
        public Map()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Name);
            Map(s => s.DepartmentId);
            References(s => s.Department).Column(nameof(DepartmentId))
                                         .Not.Insert()
                                         .Not.Update()
                                         .LazyLoad()
                ;
        }
    }
}