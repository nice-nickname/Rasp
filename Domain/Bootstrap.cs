using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Incoding.Core.CQRS.Core;

namespace Domain
{
    public class Inc : QueryBase<int>
    {
        protected override int ExecuteResult()
        {
            return 1;
        }
    }

    public class Bootstrap
    {
    }
}
