using Domain.Export;
using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using Domain.Common;

namespace UI.Controllers;

//[Authorize(Roles = "Rasp.Admin")]
[AllowAnonymous]
[Route("export")]
public class ExportController : Controller
{
    private readonly IDispatcher _dispatcher;

    public ExportController(IDispatcher dispatcher)
    {
        this._dispatcher = dispatcher;
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

    [HttpPost]
    [Route("html/zip")]
    public IActionResult AsZipHtml([FromBody] ZipHtmlModel model)
    {
        var zipStream = new MemoryStream();
        var archive = new ZipArchive(zipStream, ZipArchiveMode.Create);
        foreach (var week in model.Weeks)
        {
            foreach (var file in model.Items.Select(item => this._dispatcher.Query(new ExportScheduleAsMemoryStreamQuery
                     {
                             AuditoriumId = item.AuditoriumId,
                             TeacherId = item.TeacherId,
                             GroupId = item.GroupId,
                             FacultyId = model.FacultyId,
                             Week = week
                     })))
            {
                using var archiveFile = archive.CreateEntry(file.FileName + ".htm", CompressionLevel.Fastest)
                                               .Open();
                file.Html.CopyTo(archiveFile);
            }
        }

        zipStream.Seek(0, SeekOrigin.Begin);
        return File(zipStream, "application/zip", "Schedule.zip");
    }
}
