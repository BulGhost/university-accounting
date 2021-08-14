using System;
using StudentAccounting.Repositories.Interfaces;

namespace StudentAccounting.Data
{
    public interface IUnitOfWork : IDisposable
    {
        ICourseRepository Courses { get; }
        IGroupRepository Groups { get; }
        IStudentRepository Students { get; }
        int Complete();
    }
}
