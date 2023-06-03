using Incoding.Core.Extensions.LinqSpecs;
using System.Linq.Expressions;

namespace Domain.Persistence.Specification;

public partial class Share
{
    public interface IEntityHasDepartment
    {
        int DepartmentId { get; set; }

        Department Department { get; set; }
    }

    public interface IEntityMayHaveDepartment
    {
        int? DepartmentId { get; set; }

        Department? Department { get; set; }
    }

    public partial class Where
    {
        public class ByDepartment<T> : Specification<T> where T : IEntityHasDepartment
        {
            private readonly int _departmentId;

            public ByDepartment(int departmentId)
            {
                this._departmentId = departmentId;
            }

            public override Expression<Func<T, bool>> IsSatisfiedBy()
            {
                return s => s.DepartmentId == this._departmentId;
            }
        }

        public class ByFacultyThoughDepartment<T> : Specification<T> where T : IEntityHasDepartment
        {
            private readonly int _facultyId;

            public ByFacultyThoughDepartment(int facultyId)
            {
                this._facultyId = facultyId;
            }

            public override Expression<Func<T, bool>> IsSatisfiedBy()
            {
                return s => s.Department.FacultyId == this._facultyId;
            }
        }

        public class ByNullableDepartment<T> : Specification<T> where T : IEntityMayHaveDepartment
        {
            private readonly int _departmentId;

            public ByNullableDepartment(int? departmentId)
            {
                this._departmentId = departmentId.GetValueOrDefault();
            }

            public override Expression<Func<T, bool>> IsSatisfiedBy()
            {
                return s => s.DepartmentId == this._departmentId;
            }
        }
    }
}
