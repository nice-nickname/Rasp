using Domain.Export;
using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using Domain.Common;
using System.Text;
using Incoding.Core;
using Domain.Api;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace UI.Controllers;

[AllowAnonymous]
[Route("export")]
public class ExportController : Controller
{
    private readonly IDispatcher _dispatcher;

    private readonly IValidator<ZipHtmlModel> _validator;

    public ExportController(IDispatcher dispatcher, IValidator<ZipHtmlModel> validator)
    {
        this._dispatcher = dispatcher;
        this._validator = validator;
    }

    [Authorize(Roles = "Rasp.Admin")]
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

    [Route("html")]
    [HttpGet]
    public IActionResult AsHtml([FromQuery] int facultyId, [FromQuery] int? groupId, [FromQuery] int? auditoriumId, [FromQuery] int? teacherId, [FromQuery] int? week)
    {
        var file = this._dispatcher.Query(new ExportScheduleAsMemoryStreamQuery
        {
                AuditoriumId = auditoriumId,
                TeacherId = teacherId,
                GroupId = groupId,
                FacultyId = facultyId,
                Week = week.GetValueOrDefault(1)
        });
        return File(file.Html, "text/html", file.FileName + ".htm");
    }

    [Route("html/zip")]
    public IActionResult AsZipHtml([FromForm] ZipHtmlModel model)
    {
        var items = new List<ExportScheduleItem>();

        var validation = _validator.Validate(model);
        if (!validation.IsValid)
        {
            validation.AddToModelState(this.ModelState, "");
            this.ViewData["FacultyId"] = Request.Cookies[GlobalSelectors.FacultyId];
            return View("Index");
        }

        items.AddRange(model.Auditoriums.Select(s => new ExportScheduleItem
        {
                AuditoriumId = s
        }));
        items.AddRange(model.Teachers.Select(s => new ExportScheduleItem
        {
                TeacherId = s
        }));
        items.AddRange(model.Groups.Select(s => new ExportScheduleItem
        {
                GroupId = s
        }));

        byte[]? zip;
        using (var zipStream = new MemoryStream())
        {
            using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create))
            {
                zipStream.Position = 0;

                var firstEntry = zipArchive.CreateEntry(".schedule", CompressionLevel.Optimal);
                using (var stream = firstEntry.Open())
                {
                    stream.Write(this._dispatcher.Query(new ExportScheduleAsMemoryStreamQuery
                    {
                            AuditoriumId = items[0].AuditoriumId,
                            TeacherId = items[0].TeacherId,
                            GroupId = items[0].GroupId,
                            FacultyId = model.FacultyId,
                            Week = model.StartWeek,
                    }).Html.ToArray());
                }

                for (var week = model.StartWeek; week <= model.EndWeek; week++)
                {
                    foreach (var file in items.Select(item => this._dispatcher.Query(new ExportScheduleAsMemoryStreamQuery
                             {
                                     AuditoriumId = item.AuditoriumId,
                                     TeacherId = item.TeacherId,
                                     GroupId = item.GroupId,
                                     FacultyId = model.FacultyId,
                                     Week = week
                             })))
                    {
                        file.Html.Position = 0;
                        var entry = zipArchive.CreateEntry(file.FileName + ".htm", CompressionLevel.Optimal);
                        using (var entryStream = entry.Open())
                        {
                            file.Html.CopyTo(entryStream);
                        }
                    }
                }

                zipStream.Position = 0;
            }

            zip = zipStream.ToArray();
        }

        return File(zip, "application/zip", "Schedule.zip");
    }
}
