using System.Linq.Expressions;
using Incoding.Core.Extensions.LinqSpecs;

namespace Domain.Persistence.Specification;

public partial class Share
{
    public interface IEntityHasFaculty
    {
        int FacultyId { get; set; }

        Faculty Faculty { get; set; }
    }

    public partial class Where
    {
        public class ByFaculty<T> : Specification<T> where T : IEntityHasFaculty
        {
            private readonly int _facultyId;

            public ByFaculty(int facultyId)
            {
                this._facultyId = facultyId;
            }

            public override Expression<Func<T, bool>> IsSatisfiedBy()
            {
                return s => s.FacultyId == this._facultyId;
            }
        }
    }
}
