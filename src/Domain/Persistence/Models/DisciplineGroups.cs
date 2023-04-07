using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class DisciplineTeachers : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual int TeacherId { get; set; }

    public virtual int DisciplineId { get; set; }

    public class Mapping : ClassMap<DisciplineTeachers>
    {
        public Mapping()
        {
            Table(nameof(DisciplineTeachers));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.DisciplineId);
            Map(s => s.TeacherId);
        }
    }
}

public class DisciplineGroups : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual int GroupId { get; set; }

    public virtual int DisciplineId { get; set; }

    public class Mapping : ClassMap<DisciplineGroups>
    {
        public Mapping()
        {
            Table(nameof(DisciplineGroups));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.DisciplineId);
            Map(s => s.GroupId);
        }
    }
}