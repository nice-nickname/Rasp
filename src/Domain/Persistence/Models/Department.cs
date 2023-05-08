using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class Department : IncEntityBase, Share.IEntityHasFaculty, Share.IEntityHasId
{
    public new virtual int Id { get; set; }

    public virtual string Code { get; set; }

    public virtual string Name { get; set; }

    public virtual int FacultyId { get; set; }

    public virtual Faculty Faculty { get; set; }

    public Department()
    {
        Code = string.Empty;
        Name = string.Empty;
    }

    public class Mapping : ClassMap<Department>
    {
        public Mapping()
        {
            Table(nameof(Department));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Code);
            Map(s => s.Name);
            Map(s => s.FacultyId);

            References(s => s.Faculty).Column(nameof(FacultyId))
                                      .ReadOnly()
                                      .LazyLoad();
        }
    }
}
