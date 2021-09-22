using System;
using System.Linq;
using UniversityAccounting.DAL.EF;
using UniversityAccounting.DAL.Entities;

namespace UniversityAccounting.WEB.AutomatedUITests
{
    public class ContextFixture : IDisposable
    {
        private readonly UniversityContext _context;
        public int TestGroupId { get; }
        public string TestGroupName { get; }
        public Student TestStudent { get; set; }

        public ContextFixture()
        {
            _context = new UniversityContext();
            var group = _context.Groups.First();
            TestGroupId = group.Id;
            TestGroupName = group.Name;
        }

        public void Dispose()
        {
            DeleteAddedStudent(TestStudent);
            _context.Dispose();
        }

        private void DeleteAddedStudent(Student student)
        {
            if (student == null) return;

            var studentToDelete = _context.Students
                .SingleOrDefault(s => s.FirstName == student.FirstName &&
                                      s.LastName == student.LastName &&
                                      s.DateOfBirth == student.DateOfBirth);

            if (studentToDelete == null) return;

            _context.Students.Remove(studentToDelete);
            _context.SaveChanges();
        }
    }
}
