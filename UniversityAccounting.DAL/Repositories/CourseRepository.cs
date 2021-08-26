using UniversityAccounting.DAL.EF;
using UniversityAccounting.DAL.Interfaces;

namespace UniversityAccounting.DAL.Repositories
{
    public class CourseRepository : BaseRepository<Entities.Course>, ICourseRepository
    {
        public UniversityContext UniversityContext => Context as UniversityContext;

        public CourseRepository(UniversityContext context) : base(context)
        {
        }
    }
}