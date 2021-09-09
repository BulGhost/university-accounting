using System.Collections.Generic;
using System.Linq;
using UniversityAccounting.DAL.EF;
using UniversityAccounting.DAL.Entities;
using UniversityAccounting.DAL.Interfaces;

namespace UniversityAccounting.DAL.Repositories
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public UniversityContext UniversityContext => Context as UniversityContext;

        public CourseRepository(UniversityContext context) : base(context)
        {
        }

        public int SuitableCoursesCount(string searchText)
        {
            return FilterCourses(searchText).Count();
        }

        public IEnumerable<Course> GetRequiredCourses(string searchText, string sortProperty, int pageIndex,
            int pageSize, SortOrder sortOrder = SortOrder.Ascending)
        {
            var filteredCourses = FilterCourses(searchText);
            var expr = GetKeySelector(typeof(Course), sortProperty);

            var requiredCourses = sortOrder == SortOrder.Ascending
                ? filteredCourses.OrderBy(expr)
                : filteredCourses.OrderByDescending(expr);

            return requiredCourses.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        private IQueryable<Course> FilterCourses(string searchText)
        {
            var courses = UniversityContext.Set<Course>();
            if (string.IsNullOrEmpty(searchText)) return courses;

            searchText = searchText.ToLower();
            return courses.Where(c => c.Name.ToLower().Contains(searchText)
                                      || c.Description.ToLower().Contains(searchText));
        }
    }
}