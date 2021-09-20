using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UniversityAccounting.DAL.EF;
using UniversityAccounting.DAL.Entities;
using UniversityAccounting.DAL.Interfaces;

namespace UniversityAccounting.DAL.Repositories
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public UniversityContext UniversityContext => Context as UniversityContext;

        public StudentRepository(UniversityContext context) : base(context)
        {
        }

        public int SuitableStudentsCount(Expression<Func<Student, bool>> predicate, string searchText)
        {
            return FilterStudents(predicate, searchText).Count();
        }

        public IEnumerable<Student> GetRequiredStudents(Expression<Func<Student, bool>> predicate, string searchText, string sortProperty, int pageIndex,
            int pageSize, SortOrder sortOrder = SortOrder.Ascending)
        {
            var filteredStudents = FilterStudents(predicate, searchText);
            var expr = GetKeySelector(typeof(Student), sortProperty);

            var requiredStudents = sortOrder == SortOrder.Ascending
                ? filteredStudents.OrderBy(expr)
                : filteredStudents.OrderByDescending(expr);

            return requiredStudents.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        private IQueryable<Student> FilterStudents(Expression<Func<Student, bool>> predicate, string searchText)
        {
            var students = UniversityContext.Set<Student>().Where(predicate);
            if (string.IsNullOrEmpty(searchText)) return students;

            searchText = searchText.ToLower();
            bool isDate = DateTime.TryParse(searchText, out var date);
            return students.Where(s => s.FirstName.ToLower().Contains(searchText) ||
                                       s.LastName.ToLower().Contains(searchText) ||
                                       isDate && s.DateOfBirth == date);
        }
    }
}
