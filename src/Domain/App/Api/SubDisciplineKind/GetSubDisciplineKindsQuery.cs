using System.Drawing;
using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetSubDisciplineKindsQuery : QueryBase<List<GetSubDisciplineKindsQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {
        return Repository.Query<SubDisciplineKind>()
                         .Select(s => new Response
                         {
                                 Id = s.Id,
                                 Name = s.Name,
                                 Code = s.Code,
                                 HtmlColor = ColorTranslator.ToHtml(s.Color)
                         })
                         .ToList();
    }

    public record Response
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string HtmlColor { get; set; }
    }
}
