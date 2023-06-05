using System.Diagnostics.CodeAnalysis;
using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class Teacher : IncEntityBase, Share.IEntityHasDepartment, Share.IEntityHasId
{
    public new virtual int Id { get; set; }

    public virtual string Name { get; set; }

    public virtual string ShortName { get; set; }

    public virtual int DepartmentId { get; set; }

    public virtual Department Department { get; set; }

    public virtual IList<TeacherPreferences> Preferences { get; set; }

    [ExcludeFromCodeCoverage]
    public class Mapping : ClassMap<Teacher>
    {
        public Mapping()
        {
            Table(nameof(Teacher));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Name);
            Map(s => s.DepartmentId);
            Map(s => s.ShortName).Formula(@"
parsename(replace([Name], ' ', '.'), 3) + ' ' +
left (parsename(replace([Name], ' ', '.'), 2), 1) + '. ' + 
left (parsename(replace([Name], ' ', '.'), 1), 1) + '. '");

            References(s => s.Department).Column(nameof(DepartmentId))
                                         .ReadOnly()
                                         .LazyLoad();

            HasMany(s => s.Preferences).KeyColumn(nameof(TeacherPreferences.TeacherId))
                                       .ReadOnly()
                                       .Cascade.Delete()
                                       .LazyLoad();
        }
    }
}
