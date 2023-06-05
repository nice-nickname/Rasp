using System.Globalization;
using Domain.Api;
using Domain.App.Api;
using Domain.Persistence;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetDateFromWeekQuery), "Query")]
class When_executing_get_date_from_week : query_spec<GetDateFromWeekQuery, DateTime>
{
    static DateTime semesterStartDate;

    static DateTime startOfYearOnMonday;

    static Calendar calendar;

    Establish context = () =>
    {
        query = new GetDateFromWeekQuery { Week = Pleasure.Generator.PositiveNumber(1, 30), FacultyId = 1 };

        startOfYearOnMonday = new DateTime(1996, 1, 1);

        calendar = CultureInfo.InvariantCulture.Calendar;

        mock = query => mockQuery = MockQuery<GetDateFromWeekQuery, DateTime>
                                    .When(query)
                                    .StubQuery(new GetFacultySettingQuery<DateTime>
                                               {
                                                       FacultyId = query.FacultyId,
                                                       Type = FacultySettings.OfType.StartDate
                                               },
                                               semesterStartDate);
    };

    class when_start_is_monday
    {
        Establish context = () =>
        {
            semesterStartDate = startOfYearOnMonday;

            mock(query);
        };

        Because of = () => mockQuery.Execute();

        It should_be_next_monday = () => mockQuery.ShouldBeIsResult(date => date.DayOfWeek.ShouldEqual(DayOfWeek.Monday));

        It should_be_on_next_week = () => mockQuery.ShouldBeIsResult(date =>
        {
            var nextWeek = calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            var startWeekPlusWeeks = calendar.GetWeekOfYear(semesterStartDate, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            nextWeek.ShouldEqual(startWeekPlusWeeks + query.Week - 1);
        });
    }

    class when_start_is_sunday
    {
        Establish context = () =>
        {
            semesterStartDate = startOfYearOnMonday.AddDays(8);

            mock(query);
        };

        Because of = () => mockQuery.Execute();

        It should_be_next_monday = () => mockQuery.ShouldBeIsResult(date => date.DayOfWeek.ShouldEqual(DayOfWeek.Monday));

        It should_be_on_next_week = () => mockQuery.ShouldBeIsResult(date =>
        {
            var nextWeek = calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            var startWeekPlusWeeks = calendar.GetWeekOfYear(semesterStartDate, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            nextWeek.ShouldEqual(startWeekPlusWeeks + query.Week - 1);
        });
    }

    class when_start_is_in_middle_on_week
    {
        Establish context = () =>
        {
            semesterStartDate = startOfYearOnMonday.AddDays(4);

            mock(query);
        };

        Because of = () => mockQuery.Execute();

        It should_be_next_monday = () => mockQuery.ShouldBeIsResult(date => date.DayOfWeek.ShouldEqual(DayOfWeek.Monday));

        It should_be_on_next_week = () => mockQuery.ShouldBeIsResult(date =>
        {
            var nextWeek = calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            var startWeekPlusWeeks = calendar.GetWeekOfYear(semesterStartDate, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            nextWeek.ShouldEqual(startWeekPlusWeeks + query.Week - 1);
        });
    }
}
