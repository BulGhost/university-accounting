using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniversityAccounting.DAL.EF;
using UniversityAccounting.DAL.Repositories;

namespace UniversityAccounting.DAL.BusinessLogic
{
    public class DuplicateVerifier
    {
        private readonly UniversityContext _context;

        public DuplicateVerifier()
        {
            var dbContextOptions = new DbContextOptions<UniversityContext>();
            _context = new UniversityContext(dbContextOptions);
        }

        public bool VerifyCourseName(int id, string name)
        {
            var courseRepository = new CourseRepository(_context);

            if (id == 0) return !courseRepository.Find(c => c.Name == name).Any();

            var coursesWithSameName = courseRepository.Find(c => c.Name == name);
            return coursesWithSameName.All(course => course.Id == id);
        }

        public bool VerifyGroupName(int id, string name)
        {
            var groupRepository = new GroupRepository(_context);

            if (id == 0) return !groupRepository.Find(g => g.Name == name).Any();

            var groupsWithSameName = groupRepository.Find(g => g.Name == name);
            return groupsWithSameName.All(group => group.Id == id);
        }

        public bool VerifyStudent(int id, string firstName, string lastName, DateTime dateOfBirth)
        {
            var studentRepository = new StudentRepository(_context);

            if (id == 0)
                return !studentRepository.Find(s =>
                    s.FirstName == firstName && s.LastName == lastName && s.DateOfBirth == dateOfBirth).Any();

            var studentsWithSameAttributes = studentRepository.Find(s =>
                s.FirstName == firstName && s.LastName == lastName && s.DateOfBirth == dateOfBirth);
            return studentsWithSameAttributes.All(student => student.Id == id);
        }
    }
}
