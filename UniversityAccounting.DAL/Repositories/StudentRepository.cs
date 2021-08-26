using UniversityAccounting.DAL.EF;
using UniversityAccounting.DAL.Interfaces;

namespace UniversityAccounting.DAL.Repositories
{
    public class StudentRepository : BaseRepository<Entities.Student>, IStudentRepository
    {
        public UniversityContext UniversityContext => Context as UniversityContext;

        public StudentRepository(UniversityContext context) : base(context)
        {
        }
    }
}
