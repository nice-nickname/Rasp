using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

using System.ComponentModel;
using Resources;

public class SubDiscipline : IncEntityBase
{
    public enum OfType
    {
        [Description(nameof(DataResources.Lecture))]
        LECTURE,

        [Description(nameof(DataResources.Practice))]
        PRACTICE,

        [Description(nameof(DataResources.Lab))]
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
            Table(nameof(SubDiscipline));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Hours);
            Map(s => s.DisciplineId);
            Map(s => s.Type).CustomType<OfType>();

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
