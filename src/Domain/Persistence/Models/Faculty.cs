﻿using System.Diagnostics.CodeAnalysis;
using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class Faculty : IncEntityBase, Share.IEntityHasId
{
    public new virtual int Id { get; set; }

    public virtual string Code { get; set; }

    public virtual string Name { get; set; }

    public virtual IList<FacultySettings> Settings { get; set; }

    [ExcludeFromCodeCoverage]
    public class Mapping : ClassMap<Faculty>
    {
        public Mapping()
        {
            Table(nameof(Faculty));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Code);
            Map(s => s.Name);

            HasMany(s => s.Settings).KeyColumn(nameof(FacultySettings.FacultyId))
                                    .LazyLoad();
        }
    }
}
