using StudentAccounting.Data;
using StudentAccounting.Repositories.Interfaces;

namespace StudentAccounting.Repositories
{
    public class StudentRepository : BaseRepository<Models.Student>, IStudentRepository
    {
        public UniversityContext UniversityContext => Context as UniversityContext;

        public StudentRepository(UniversityContext context) : base(context)
        {
        }
    }
}
