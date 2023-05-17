using System.Text;
using Domain.Api;
using Domain.Common;
using Domain.Infrastructure;
using Incoding.Core.Block.IoC;
using Incoding.Core.CQRS.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Export;

public class ExportScheduleAsMemoryStreamQuery : QueryBase<ExportScheduleAsMemoryStreamQuery.Response>
{
    public int FacultyId { get; set; }

    public int? GroupId { get; set; }

    public int? AuditoriumId { get; set; }

    public int? TeacherId { get; set; }

    public int Week { get; set; }

    protected override Response ExecuteResult()
    {
        // TODO 17.05.2023: Вынести логику по рендеру отсюда на уровень сервиса?
        using var scope = IoCFactory.Instance.TryResolve<IServiceProvider>().CreateScope();
        var renderService = scope.ServiceProvider.GetRequiredService<IViewRenderer>();

        var name = Dispatcher.Query(new GetScheduleNameQuery
        {
                AuditoriumId = AuditoriumId,
                TeacherId = TeacherId,
                GroupId = GroupId,
        });

        var format = Dispatcher.Query(new GetScheduleFormatQuery
        {
                FacultyId = FacultyId
        });

        var schedule = Dispatcher.Query(new GetScheduleByWeekQuery
        {
                FacultyId = FacultyId,
                Week = Week,
                SelectedAuditoriumIds = GroupId.HasValue ? new[] { AuditoriumId } : Array.Empty<int?>(),
                SelectedTeacherIds = GroupId.HasValue ? new[] { TeacherId } : Array.Empty<int?>(),
                SelectedGroupIds = GroupId.HasValue ? new[] { GroupId } : Array.Empty<int?>(),
        });

        var html = renderService.Render("~/Views/Export/ExportPage.cshtml",
                                        new SchedulePageModel
                                        {
                                                Items = schedule,
                                                Format = format
                                        }).Result;

        return new Response
        {
                Html = new MemoryStream(Encoding.UTF8.GetBytes(html)),
                FileName = $"{name}_{Week}"
        };
    }

    public record Response
    {
        public MemoryStream Html { get; set; }

        public string FileName { get; set; }
    }
}
