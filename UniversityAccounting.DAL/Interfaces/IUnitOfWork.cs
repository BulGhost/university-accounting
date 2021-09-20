using System;

namespace UniversityAccounting.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICourseRepository Courses { get; }
        IGroupRepository Groups { get; }
        IStudentRepository Students { get; }
        int Complete();
    }
}
