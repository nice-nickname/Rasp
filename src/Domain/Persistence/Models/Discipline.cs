using System.ComponentModel;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class Class : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual int GroupId { get; set; }

    public virtual int AuditoriumId { get; set; }

    public virtual int SubDisciplineId { get; set; }

    public virtual int ScheduleFormatId { get; set; }

    public virtual DateTime Day { get; set; }

    public virtual Auditorium Auditorium { get; set; }

    public virtual Group Group { get; set; }

    public virtual SubDiscipline SubDiscipline { get; set; }

    public virtual ScheduleFormat ScheduleFormat { get; set; }

    public class Mapping : ClassMap<Class>
    {
        public Mapping()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.GroupId);
            Map(s => s.AuditoriumId);
            Map(s => s.SubDisciplineId);
            Map(s => s.ScheduleFormatId);
            Map(s => s.Day);

            References(s => s.Auditorium).Column(nameof(AuditoriumId))
                                         .ReadOnly()
                                         .LazyLoad();

            References(s => s.Group).Column(nameof(GroupId))
                                    .ReadOnly()
                                    .LazyLoad();

            References(s => s.SubDiscipline).Column(nameof(SubDisciplineId))
                                            .ReadOnly()
                                            .LazyLoad();

            References(s => s.ScheduleFormat).Column(nameof(ScheduleFormatId))
                                             .ReadOnly()
                                             .LazyLoad();
        }
    }
}

public class Discipline : IncEntityBase
{
    public enum OfType
    {
        [Description("Зачет")] GRADE,

        [Description("Дифференцированный зачет")]
        DIFFERENTIAL_GRADE,

        [Description("Экзамен")] EXAMINATION
    }

    public new virtual int Id { get; set; }

    public virtual string Name { get; set; }

    public virtual OfType Type { get; set; }

    public virtual int? DepartmentId { get; set; }

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
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Name);
            Map(s => s.Type).CustomType<OfType>();
            Map(s => s.DepartmentId).Nullable();

            References(s => s.Department).Column(nameof(DepartmentId))
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