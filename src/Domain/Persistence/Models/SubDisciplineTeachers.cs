﻿using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class SubDisciplineTeachers : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual int TeacherId { get; set; }

    public virtual int SubDisciplineId { get; set; }

    public class Mapping : ClassMap<SubDisciplineTeachers>
    {
        public Mapping()
        {
            Table(nameof(SubDisciplineTeachers));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.SubDisciplineId);
            Map(s => s.TeacherId);
        }
    }
}