using StudentAccounting.Repositories;
using StudentAccounting.Repositories.Interfaces;

namespace StudentAccounting.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UniversityContext _context;

        public UnitOfWork(UniversityContext context)
        {
            _context = context;
            Courses = new CourseRepository(_context);
            Groups = new GroupRepository(_context);
            Students = new StudentRepository(_context);
        }

        public ICourseRepository Courses { get; }
        public IGroupRepository Groups { get; }
        public IStudentRepository Students { get; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
