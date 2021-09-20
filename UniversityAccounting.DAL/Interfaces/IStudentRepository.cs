using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UniversityAccounting.DAL.Entities;

namespace UniversityAccounting.DAL.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        int SuitableStudentsCount(Expression<Func<Student, bool>> predicate, string searchText);

        IEnumerable<Student> GetRequiredStudents(Expression<Func<Student, bool>> predicate, string searchText,
            string sortProperty, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending);
    }
}
