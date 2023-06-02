using Domain.Api;
using Domain.Persistence;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetWeekendsForWeekQuery), "Query")]
class When_executing_get_weekends_for_week : query_spec<GetWeekendsForWeekQuery, List<DateOnly>>
{
    Establish context = () => 
    {
        query = new GetWeekendsForWeekQuery { FacultyId = Pleasure.Generator.PositiveNumber(), StartDate = TestDataHelper.YearWithFirstDayMonday };
        
        var weekendItems = new List<DateOnly>
        {
            TestDataHelper.YearWithFirstDayMondayDateOnly,
            TestDataHelper.YearWithFirstDayMondayDateOnly.AddDays(1),
            TestDataHelper.YearWithFirstDayMondayDateOnly.AddDays(6),
            TestDataHelper.YearWithFirstDayMondayDateOnly.AddDays(7),
            TestDataHelper.YearWithFirstDayMondayDateOnly.AddDays(8),
            TestDataHelper.YearWithFirstDayMondayDateOnly.AddDays(10),
            TestDataHelper.YearWithFirstDayMondayDateOnly.AddDays(14)
        };

        weekends = new[]
        {
            new Holidays { Date = weekendItems[0] },
            new Holidays { Date = weekendItems[3] },
            new Holidays { Date = weekendItems[5] }
        };

        mockQuery = MockQuery<GetWeekendsForWeekQuery, List<DateOnly>>
                        .When(query)
                        .StubQuery(new GetScheduleFormatQuery { FacultyId = query.FacultyId }, new GetScheduleFormatQuery.Response
                        {
                            StartDate = query.StartDate,
                            EndDate = query.StartDate.AddDays(7)
                        })
                        .StubQuery(entities: weekends);
    };

    Because of = () => mockQuery.Execute();

    It should_return_holidays = () => mockQuery.ShouldBeIsResult(result => result.ShouldEqual(weekends.Select(s => s.Date)));

    static Holidays[] weekends;
}
