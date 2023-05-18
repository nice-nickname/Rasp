using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetExportSearchQuery : QueryBase<List<GetExportSearchQuery.Response>>
{
    public int FacultyId { get; set; }

    public string Search { get; set; }

    protected override List<Response> ExecuteResult()
    {
        var teachers = Repository.Query<Teacher>()
                                 .Where(s => s.Department.FacultyId == FacultyId)
                                 .Where(s => s.Name.Contains(Search))
                                 .Select(s => new Response
                                 {
                                         Id = s.Id,
                                         Name = s.Name,
                                         Type = OfType.TEACHER
                                 })
                                 .OrderBy(s => s.Name)
                                 .ToList();

        var groups = Repository.Query<Group>()
                               .Where(s => s.Department.FacultyId == FacultyId)
                               .Where(s => s.Code.Contains(Search))
                               .OrderBy(s => s.Course)
                               .ThenBy(s => s.Code)
                               .Select(s => new Response
                               {
                                       Id = s.Id,
                                       Name = s.Code,
                                       Type = OfType.GROUP
                               })
                               .ToList();

        List<Response> auditoriums;

        if (Search.Contains("-"))
        {
            var building = Search.Split("-")[0];
            var auditorium = Search.Split("-")[1];

            auditoriums = Repository.Query<Auditorium>()
                                    .Where(s => s.Building.Name == building && s.Code.Contains(auditorium))
                                    .OrderBy(s => s.Building.Name)
                                    .ThenBy(s => s.Code)
                                    .Select(s => new Response
                                    {
                                            Id = s.Id,
                                            Name = $"{s.Building.Name}-{s.Code}",
                                            Type = OfType.AUDITORIUM,
                                    })
                                    .ToList();
        }
        else
        {
            auditoriums = Repository.Query<Auditorium>()
                                    .Where(s => s.Code.Contains(Search))
                                    .OrderBy(s => s.Building.Name)
                                    .ThenBy(s => s.Code)
                                    .Select(s => new Response
                                    {
                                            Id = s.Id,
                                            Name = $"{s.Building.Name}-{s.Code}",
                                            Type = OfType.AUDITORIUM,
                                    })
                                    .ToList();
        }

        teachers.AddRange(groups);
        teachers.AddRange(auditoriums);
        return teachers;
    }

    public record Response
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public OfType Type { get; set; }
    }

    public enum OfType
    {
        TEACHER,
        GROUP,
        AUDITORIUM
    }
}
