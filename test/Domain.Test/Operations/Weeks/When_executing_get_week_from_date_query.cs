using System.Globalization;
using Domain.Api;
using Domain.App.Api;
using Domain.Persistence;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetWeekFromDateQuery), "Query")]
class When_executing_get_week_from_date : query_spec<GetWeekFromDateQuery, int>
{
    static DateTime semesterStartDate;

    static DateTime yearFirstDayMonday;

    static int semesterWeeksCount;

    static Calendar calendar;

    Establish context = () =>
    {
        query = new GetWeekFromDateQuery { FacultyId = 1 };

        yearFirstDayMonday = new DateTime(1996, 1, 1);

        semesterStartDate = yearFirstDayMonday;

        mock = query => mockQuery = MockQuery<GetWeekFromDateQuery, int>
                                    .When(query)
                                    .StubQuery(new GetFacultySettingQuery<DateTime>
                                               {
                                                       FacultyId = query.FacultyId,
                                                       Type = FacultySettings.OfType.StartDate
                                               },
                                               semesterStartDate)
                                    .StubQuery(new GetFacultySettingQuery<int>
                                               {
                                                       FacultyId = query.FacultyId,
                                                       Type = FacultySettings.OfType.CountOfWeeks
                                               },
                                               semesterWeeksCount);

        calendar = CultureInfo.InvariantCulture.Calendar;
    };

    class when_date_is_start_of_semester
    {
        Establish context = () =>
        {
            query.Date = semesterStartDate;

            mock(query);
        };

        Because of = () => mockQuery.Execute();

        It should_return_one = () => mockQuery.ShouldBeIsResult(week => week.ShouldEqual(1));
    }

    class when_date_is_before_semester_start
    {
        Establish context = () =>
        {
            query.Date = semesterStartDate.AddDays(-10);

            mock(query);
        };

        Because of = () => mockQuery.Execute();

        It should_return_one = () => mockQuery.ShouldBeIsResult(week => week.ShouldEqual(1));
    }

    class when_date_is_in_semester
    {
        static int expectedWeek;

        Establish context = () =>
        {
            var days = Pleasure.Generator.PositiveNumber(0, 365);
            query.Date = semesterStartDate.AddDays(days);

            expectedWeek = calendar.GetWeekOfYear(query.Date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            mock(query);
        };

        Because of = () => mockQuery.Execute();

        It should_be_valid_week = () => mockQuery.ShouldBeIsResult(week => week.ShouldEqual(expectedWeek));
    }
}
