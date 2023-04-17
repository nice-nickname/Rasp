using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class SubDiscipline : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual int Hours { get; set; }

    public virtual int KindId { get; set; }

    public virtual int DisciplineId { get; set; }

    public virtual SubDisciplineKinds Kind { get; set; }

    public virtual Discipline Discipline { get; set; }

    public virtual IList<Teacher> Teachers { get; set; }

    public SubDiscipline()
    {
        Teachers = new List<Teacher>();
    }

    public class Mapping : ClassMap<SubDiscipline>
    {
        public Mapping()
        {
            Table(nameof(SubDiscipline));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Hours);
            Map(s => s.DisciplineId);
            Map(s => s.KindId);

            References(s => s.Kind).Column(nameof(KindId))
                                   .LazyLoad()
                                   .ReadOnly();

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
