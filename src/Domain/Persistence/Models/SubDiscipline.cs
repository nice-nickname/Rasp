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

    public virtual int DisciplineId { get; set; }

    public virtual Discipline Discipline { get; set; }

    public virtual IList<Teacher> Teachers { get; set; }

    public class Mapping : ClassMap<SubDiscipline>
    {
        public Mapping()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Type).CustomType<OfType>();
            Map(s => s.Hours);

            References(s => s.Discipline).Column(nameof(DisciplineId))
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