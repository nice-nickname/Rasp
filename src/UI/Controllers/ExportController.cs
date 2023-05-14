using Domain.Api;
using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Common.Models;

namespace UI.Controllers;

[AllowAnonymous]
[Route("export")]
public class ExportController : Controller
{
    private readonly IDispatcher _dispatcher;

    public ExportController(IDispatcher dispatcher)
    {
        this._dispatcher = dispatcher;
    }

    public IActionResult Index()
    {
        var faculties = this._dispatcher.Query(new GetFacultiesQuery());

        if (faculties.Count < 1)
        {
            return StatusCode(500, "Возможность работы без факультетов невозможна :(");
        }

        var facultyId = HttpContext.Request.Cookies[GlobalSelectors.FacultyId] ?? string.Empty;

        if (!HttpContext.Request.Cookies.ContainsKey(GlobalSelectors.FacultyId) || faculties.All(s => s.Id.ToString() != HttpContext.Request.Cookies[GlobalSelectors.FacultyId]))
        {
            HttpContext.Response.Cookies.Append(GlobalSelectors.FacultyId, faculties.First().Id.ToString());
            facultyId = faculties.First().Id.ToString();
        }

        this.ViewData["FacultyId"] = facultyId;
        return View();
    }

    [Route("/schedule/{facultyId}/{type}/{id}")]
    public IActionResult Schedule([FromRoute] int facultyId, [FromRoute] GetExportSearchQuery.OfType type, [FromRoute] int id, [FromQuery] int? week)
    {
        var faculties = this._dispatcher.Query(new GetFacultiesQuery());

        if (faculties.Count < 1)
        {
            return StatusCode(500, "Возможность работы без факультетов невозможна :(");
        }

        if (faculties.All(s => s.Id != facultyId))
        {
            HttpContext.Response.Cookies.Append(GlobalSelectors.FacultyId, faculties.First().Id.ToString());
            facultyId = faculties.First().Id;
        }
        var result = this._dispatcher.Query(new GetScheduleByWeekQuery
        {
                Week = week.GetValueOrDefault(1),
                FacultyId = facultyId,
                SelectedAuditoriumId = type == GetExportSearchQuery.OfType.AUDITORIUM ? id : null,
                SelectedTeacherId = type == GetExportSearchQuery.OfType.TEACHER ? id : null,
                SelectedGroupId = type == GetExportSearchQuery.OfType.GROUP ? id : null,
        });
        var format = this._dispatcher.Query(new GetScheduleFormatQuery
        {
                FacultyId = facultyId
        });

        return View(new SchedulePageModel { Format = format, Items = result, Title = "", ActiveWeek = week.GetValueOrDefault(1) });
    }
}
