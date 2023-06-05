using Domain.Api;
using Domain.Persistence;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetWeekendsForWeekQuery), "Query")]
class When_executing_get_weekends_for_week : query_spec<GetWeekendsForWeekQuery, List<DateOnly>>
{
    static Holidays[] weekends;

    Establish context = () =>
    {
        query = new GetWeekendsForWeekQuery { FacultyId = Pleasure.Generator.PositiveNumber(), StartDate = TestDataHelper.YearWithFirstDayMonday };

        weekends = new[]
        {
                new Holidays { Date = TestDataHelper.YearWithFirstDayMondayDateOnly.AddDays(0) },
                new Holidays { Date = TestDataHelper.YearWithFirstDayMondayDateOnly.AddDays(3) },
                new Holidays { Date = TestDataHelper.YearWithFirstDayMondayDateOnly.AddDays(6) }
        };

        mockQuery = MockQuery<GetWeekendsForWeekQuery, List<DateOnly>>
                    .When(query)
                    .StubQuery(new GetScheduleFormatQuery { FacultyId = query.FacultyId },
                               new GetScheduleFormatQuery.Response
                               {
                                       StartDate = query.StartDate,
                                       EndDate = query.StartDate.AddDays(7)
                               })
                    .StubQuery(entities: weekends);
    };

    Because of = () => mockQuery.Execute();

    It should_return_holidays = () => mockQuery.ShouldBeIsResult(result => result.ShouldEqual(weekends.Select(s => s.Date)));
}
