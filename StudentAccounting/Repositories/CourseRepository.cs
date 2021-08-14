using StudentAccounting.Data;
using StudentAccounting.Repositories.Interfaces;

namespace StudentAccounting.Repositories
{
    public class CourseRepository : BaseRepository<Models.Course>, ICourseRepository
    {
        public UniversityContext UniversityContext => Context as UniversityContext;

        public CourseRepository(UniversityContext context) : base(context)
        {
        }
    }
}