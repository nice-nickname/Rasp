using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class Discipline : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual string Name { get; set; }

    public virtual string Code { get; set; }

    public virtual int KindId { get; set; }

    public virtual int? DepartmentId { get; set; }

    public virtual DisciplineKinds Kind { get; set; }

    public virtual Department? Department { get; set; }

    public virtual IList<Group> Groups { get; set; }

    public virtual IList<Teacher> Teachers { get; set; }

    public Discipline()
    {
        Groups = new List<Group>();
        Teachers = new List<Teacher>();
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
;           Map(s => s.KindId);

            References(s => s.Department).Column(nameof(DepartmentId))
                                         .ReadOnly()
                                         .Nullable()
                                         .LazyLoad();

            References(s => s.Kind).Column(nameof(KindId))
                                   .ReadOnly()
                                   .LazyLoad();

            HasManyToMany(s => s.Groups).Table(nameof(DisciplineGroups))
                                        .ParentKeyColumn(nameof(DisciplineGroups.DisciplineId))
                                        .ChildKeyColumn(nameof(DisciplineGroups.GroupId))
                                        .LazyLoad()
                                        .ReadOnly();

            HasManyToMany(s => s.Teachers).Table(nameof(DisciplineTeachers))
                                          .ParentKeyColumn(nameof(DisciplineTeachers.DisciplineId))
                                          .ChildKeyColumn(nameof(DisciplineTeachers.TeacherId))
                                          .LazyLoad()
                                          .ReadOnly();
        }
    }
}
