using Domain.Api;
using Domain.App.Api;
using Domain.Common;
using Domain.Export;
using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using UI.Common.Models;

namespace UI.Controllers;

[AllowAnonymous]
[Route("view")]
public class ViewController : Controller
{
    private readonly IDispatcher _dispatcher;

    public ViewController(IDispatcher dispatcher)
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

    [Route("public/{facultyId}/{type}/{id}")]
    public IActionResult Public([FromRoute] int facultyId, [FromRoute] GetExportSearchQuery.OfType type, [FromRoute] int id, [FromQuery] int? week)
    {
        var faculties = this._dispatcher.Query(new GetFacultiesQuery());

        week ??= this._dispatcher.Query(new GetWeekFromDateQuery { FacultyId = facultyId, Date = DateTime.Now });

        if (faculties.Count < 1)
        {
            return StatusCode(500, "Возможность работы без факультетов невозможна :(");
        }

        if (faculties.All(s => s.Id != facultyId))
        {
            HttpContext.Response.Cookies.Append(GlobalSelectors.FacultyId, faculties.First().Id.ToString());
            return NotFound();
        }

        var result = this._dispatcher.Query(new GetScheduleByWeekQuery
        {
                Week = week.GetValueOrDefault(1),
                FacultyId = facultyId,
                SelectedAuditoriumIds = type == GetExportSearchQuery.OfType.AUDITORIUM ? new int?[] { id } : Array.Empty<int?>(),
                SelectedTeacherIds = type == GetExportSearchQuery.OfType.TEACHER ? new int?[] { id } : Array.Empty<int?>(),
                SelectedGroupIds = type == GetExportSearchQuery.OfType.GROUP ? new int?[] { id } : Array.Empty<int?>(),
        });
        var format = this._dispatcher.Query(new GetScheduleFormatQuery
        {
                FacultyId = facultyId
        });

        var name = this._dispatcher.Query(new GetScheduleNameQuery
        {
                AuditoriumId = type == GetExportSearchQuery.OfType.AUDITORIUM ? id : null,
                TeacherId = type == GetExportSearchQuery.OfType.TEACHER ? id : null,
                GroupId = type == GetExportSearchQuery.OfType.GROUP ? id : null
        });

        return View(new SchedulePageModel { Format = format, Items = result, Title = $"{DataResources.Schedule} {name}", ActiveWeek = week.Value });
    }
}
