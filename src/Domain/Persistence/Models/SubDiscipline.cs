using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class SubDiscipline : IncEntityBase
{
    public enum OfType
    {
        LECTURE,

        PRACTICE,

        LAB,

        CONSULTANT,

        EXAMINATION
    }

    public new virtual int Id { get; set; }

    public virtual int Hours { get; set; }

    public virtual OfType Type { get; set; }

    public virtual IList<Group> Groups { get; set; }

    public virtual IList<Teacher> Teachers { get; set; }

    public class Mapping : ClassMap<SubDiscipline>
    {
        public Mapping()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Type).CustomType<OfType>();
            Map(s => s.Hours);

            HasManyToMany(s => s.Groups).Table(nameof(SubDisciplineGroups))
                                        .ParentKeyColumn(nameof(SubDisciplineGroups.SubDisciplineId))
                                        .ChildKeyColumn(nameof(SubDisciplineGroups.GroupId))
                                        .LazyLoad()
                                        .ReadOnly();

            HasManyToMany(s => s.Teachers).Table(nameof(SubDisciplineTeachers))
                                          .ParentKeyColumn(nameof(SubDisciplineTeachers.SubDisciplineId))
                                          .ChildKeyColumn(nameof(SubDisciplineTeachers.TeacherId))
                                          .LazyLoad()
                                          .ReadOnly();
        }
    }
}