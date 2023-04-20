using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;

namespace Domain.Api;

public class GetAuditoriumKindsQuery : QueryBase<List<GetAuditoriumKindsQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {
        return Repository.Query<AuditoriumKind>()
                         .Select(r => new Response
                         {
                                 Id = r.Id,
                                 Kind = r.Kind
                         })
                         .ToList();
    }

    public class Response
    {
        public int Id { get; set; }

        public string Kind { get; set; }
    }
}

public class GetAuditoriumKindsForMultiselectQuery : QueryBase<List<KeyValueVm>>
{
    public int[] Ids { get; set; }

    protected override List<KeyValueVm> ExecuteResult()
    {
        return Repository.Query<AuditoriumKind>()
                         .Select(r => new KeyValueVm
                         {
                                 Text = r.Kind,
                                 Value = r.Id.ToString(),
                                 Selected = Ids.Contains(r.Id)
                         })
                         .ToList();
    }
}
