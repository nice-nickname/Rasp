using System.Linq.Expressions;
using Domain.Persistence.Mappers;
using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;
using Incoding.Core.Extensions.LinqSpecs;

namespace Domain.Persistence;

public class Class : IncEntityBase, Share.IEntityHasId
{
    public new virtual int Id { get; set; }

    public virtual int Week { get; set; }

    public virtual DayOfWeek Day { get; set; }

    public virtual int SubGroupNo { get; set; }

    public virtual int? AuditoriumId { get; set; }

    public virtual int ScheduleFormatId { get; set; }

    public virtual int DisciplinePlanId { get; set; }

    public virtual bool IsUnwanted { get; set; }

    public virtual Auditorium? Auditorium { get; set; }

    public virtual ScheduleFormat ScheduleFormat { get; set; }

    public virtual DisciplinePlan Plan { get; set; }

    public class Mapping : ClassMap<Class>
    {
        public Mapping()
        {
            Table(nameof(Class));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Week);
            Map(s => s.SubGroupNo);
            Map(s => s.Day).CustomType<NHibernateDayOfWeekEnumMapper>().Not.Nullable();
            Map(s => s.AuditoriumId).Nullable();
            Map(s => s.ScheduleFormatId);
            Map(s => s.DisciplinePlanId);
            Map(s => s.IsUnwanted);

            References(s => s.Auditorium).Column(nameof(AuditoriumId))
                                         .Nullable()
                                         .ReadOnly()
                                         .LazyLoad();

            References(s => s.ScheduleFormat).Column(nameof(ScheduleFormatId))
                                             .ReadOnly()
                                             .LazyLoad();

            References(s => s.Plan).Column(nameof(DisciplinePlanId))
                                   .ReadOnly()
                                   .LazyLoad();
        }
    }

    public class Where
    {
        public class ByFaculty : Specification<Class>
        {
            private readonly int _facultyId;

            public ByFaculty(int facultyId)
            {
                this._facultyId = facultyId;
            }

            public override Expression<Func<Class, bool>> IsSatisfiedBy()
            {
                return s => s.ScheduleFormat.FacultyId == _facultyId;
            }
        }

        public class ByWeek : Specification<Class>
        {
            private readonly int _week;

            public ByWeek(int week)
            {
                this._week = week;
            }

            public override Expression<Func<Class, bool>> IsSatisfiedBy()
            {
                return s => s.Week == _week;
            }
        }
    }
}
