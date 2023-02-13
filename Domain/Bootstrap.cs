using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Data;

namespace Domain
{
    public class Roles : IncEntityBase
    {
        public new virtual int Id { get; set; }

        public virtual string? Role { get; set; }

        public class Map : ClassMap<Roles>
        {
            public Map()
            {
                Table("Roles");
                Id(s => s.Id, "idRole");
                Map(s => s.Role, "role");
            }
        }
    }

    public class Inc : QueryBase<int>
    {
        protected override int ExecuteResult()
        {
            return 123;
        }
    }

    public class Bootstrap
    {
    }
}
