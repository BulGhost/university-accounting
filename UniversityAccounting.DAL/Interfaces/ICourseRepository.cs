using System.Collections.Generic;
using UniversityAccounting.DAL.Entities;

namespace UniversityAccounting.DAL.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        int SuitableCoursesCount(string searchText);

        IEnumerable<Course> GetRequiredCourses(string searchText, string sortProperty, int pageIndex,
            int pageSize, SortOrder sortOrder = SortOrder.Ascending);
    }
}
