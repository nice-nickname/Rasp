using Domain.Api;
using Domain.App.Api;
using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Common.Models;

namespace UI.Controllers;

[Authorize(Roles = "Rasp.Admin")]
public class ScheduleController : Controller
{
    private readonly IDispatcher _dispatcher;

    public ScheduleController(IDispatcher dispatcher)
    {
        this._dispatcher = dispatcher;
    }

    public IActionResult Index([Bind("Type", "Week")] ScheduleIndexPageModel model, [FromQuery] int? entityId)
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

        model.Week ??= this._dispatcher.Query(new GetWeekFromDateQuery
        {
                Date = DateTime.Now,
                FacultyId = int.Parse(facultyId)
        });

        if (entityId.HasValue)
        {
            var type = model.Type.GetValueOrDefault(ScheduleIndexPageModel.EntityType.GROUP);
            model.AuditoriumId = type == ScheduleIndexPageModel.EntityType.AUDITORIUM ? entityId : null;
            model.TeacherId = type == ScheduleIndexPageModel.EntityType.TEACHER ? entityId : null;
            model.GroupId = type == ScheduleIndexPageModel.EntityType.GROUP ? entityId : null;
        }

        return View(model);
    }
}
