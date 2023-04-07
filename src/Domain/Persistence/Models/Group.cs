using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class Group : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual string Code { get; set; }

    public virtual int DepartmentId { get; set; }

    public virtual Department Department { get; set; }

    public class Mapping : ClassMap<Group>
    {
        public Mapping()
        {
            Table(nameof(Group));
            Id(s => s.Id);
            Map(s => s.Code);
            Map(s => s.DepartmentId);

            References(s => s.Department).Column(nameof(DepartmentId))
                                         .ReadOnly()
                                         .LazyLoad();
        }
    }
}