using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class ClassModule : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual int ClassId { get; set; }

    public virtual int TeacherId { get; set; }

    public virtual int LectureHours { get; set; }

    public virtual int PracticeHours { get; set; }

    public virtual int LaboratoryHours { get; set; }

    public virtual Class Class { get; set; }

    public virtual Teacher Teacher { get; set; }

    internal class Map : ClassMap<ClassModule>
    {
        public Map()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.ClassId);
            Map(s => s.TeacherId);
            Map(s => s.LectureHours);
            Map(s => s.PracticeHours);
            Map(s => s.LaboratoryHours);
            References(s => s.Class).Column(nameof(ClassId))
                                    .Not.Insert()
                                    .Not.Update()
                                    .LazyLoad();
            References(s => s.Teacher).Column(nameof(TeacherId))
                                      .Not.Insert()
                                      .Not.Update()
                                      .LazyLoad();
        }
    }
}
