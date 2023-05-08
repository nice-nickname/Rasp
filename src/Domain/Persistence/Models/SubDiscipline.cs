using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class SubDiscipline : IncEntityBase, Share.IEntityHasDiscipline, Share.IEntityHasId
{
    public new virtual int Id { get; set; }

    public virtual int Hours { get; set; }

    public virtual int KindId { get; set; }

    public virtual int DisciplineId { get; set; }

    public virtual bool IsParallelHours { get; set; }

    public virtual SubDisciplineKind Kind { get; set; }

    public virtual Discipline Discipline { get; set; }

    public virtual IList<Teacher> Teachers { get; set; }

    public virtual IList<Auditorium> Auditoriums { get; set; }

    public virtual IList<AuditoriumKind> AuditoriumKinds { get; set; }

    public SubDiscipline()
    {
        Teachers = new List<Teacher>();
        Auditoriums = new List<Auditorium>();
        AuditoriumKinds = new List<AuditoriumKind>();
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
            Map(s => s.IsParallelHours);

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

            HasManyToMany(s => s.Auditoriums).Table(nameof(SubDisciplineAuditoriums))
                                             .ParentKeyColumn(nameof(SubDisciplineAuditoriums.SubDisciplineId))
                                             .ChildKeyColumn(nameof(SubDisciplineAuditoriums.AuditoriumId))
                                             .LazyLoad()
                                             .ReadOnly();

            HasManyToMany(s => s.AuditoriumKinds).Table(nameof(SubDisciplineAuditoriumKinds))
                                                 .ParentKeyColumn(nameof(SubDisciplineAuditoriumKinds.SubDisciplineId))
                                                 .ChildKeyColumn(nameof(SubDisciplineAuditoriumKinds.AuditoriumKindId))
                                                 .LazyLoad()
                                                 .ReadOnly();
        }
    }
}
