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
        return View();
    }

    [Route("/schedule/{type}/{id}")]
    public IActionResult Schedule([FromRoute] GetExportSearchQuery.OfType type, [FromRoute] int id, [FromQuery] int? week)
    {
        var facultyId = Convert.ToInt32(HttpContext.Request.Cookies[GlobalSelectors.FacultyId]);

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

        return View(new SchedulePageModel { Format = format, Items = result, Title = "" });
    }
}
