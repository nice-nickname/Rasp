using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Test;

internal static class TestDataHelper
{
    internal static readonly DateTime YearWithFirstDayMonday = new (1996, 1, 1);

    internal static readonly DateOnly YearWithFirstDayMondayDateOnly = new(1996, 1, 1);

}
