using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class Discipline : IncEntityBase, Share.IEntityMayHaveDepartment
{
    public new virtual int Id { get; set; }

    public virtual string Name { get; set; }

    public virtual string Code { get; set; }

    public virtual int KindId { get; set; }

    public virtual int? DepartmentId { get; set; }

    public virtual DisciplineKind Kind { get; set; }

    public virtual Department? Department { get; set; }

    public virtual IList<Group> Groups { get; set; }

    public virtual IList<SubDiscipline> SubDisciplines { get; set; }

    public Discipline()
    {
        Name = string.Empty;
        Code = string.Empty;
        Groups = new List<Group>();
        SubDisciplines = new List<SubDiscipline>();
    }

    public class Mapping : ClassMap<Discipline>
    {
        public Mapping()
        {
            Table(nameof(Discipline));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Name);
            Map(s => s.Code);
            Map(s => s.DepartmentId).Nullable();
            Map(s => s.KindId);

            References(s => s.Department).Column(nameof(DepartmentId))
                                         .ReadOnly()
                                         .Nullable()
                                         .LazyLoad();

            References(s => s.Kind).Column(nameof(KindId))
                                   .ReadOnly()
                                   .LazyLoad();

            HasMany(s => s.SubDisciplines).KeyColumn(nameof(SubDiscipline.DisciplineId))
                                          .LazyLoad()
                                          .ReadOnly();

            HasManyToMany(s => s.Groups).Table(nameof(DisciplineGroups))
                                        .ParentKeyColumn(nameof(DisciplineGroups.DisciplineId))
                                        .ChildKeyColumn(nameof(DisciplineGroups.GroupId))
                                        .LazyLoad()
                                        .ReadOnly();
        }
    }
}
