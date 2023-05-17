using Domain.Export;
using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using Domain.Common;
using System.Text;
using Incoding.Core;

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

    [Route("html/zip")]
    public IActionResult AsZipHtml([FromBody] ZipHtmlModel model)
    {
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
                        AuditoriumId = model.Items[0].AuditoriumId,
                        TeacherId = model.Items[0].TeacherId,
                        GroupId = model.Items[0].GroupId,
                        FacultyId = model.FacultyId,
                        Week = model.Weeks[0],
                    }).Html.ToArray());
                }

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
