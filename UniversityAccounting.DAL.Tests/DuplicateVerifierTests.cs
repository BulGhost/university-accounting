using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using FluentAssertions;
using UniversityAccounting.DAL.BusinessLogic;
using UniversityAccounting.DAL.Entities;
using UniversityAccounting.DAL.Interfaces;

namespace UniversityAccounting.DAL.Tests
{
    public class DuplicateVerifierTests
    {
        private readonly List<Course> _coursesInMemoryDb = new()
        {
            new Course {Id = 1, Name = "Course1", Description = "first"},
            new Course {Id = 2, Name = "Course2", Description = "second foo"},
            new Course {Id = 3, Name = "Course3", Description = "third"},
            new Course {Id = 4, Name = "Course4", Description = "fourth foo"},
            new Course {Id = 5, Name = "Course5", Description = "fifth"}
        };

        private readonly List<Group> _groupsInMemoryDb = new()
        {
            new Group {Id = 1, CourseId = 1, Name = "Group1", FormationDate = new DateTime(2018, 5, 23)},
            new Group {Id = 2, CourseId = 1, Name = "Group2", FormationDate = new DateTime(2019, 3, 12)},
            new Group {Id = 3, CourseId = 1, Name = "Group3", FormationDate = new DateTime(2019, 1, 5)},
            new Group {Id = 4, CourseId = 2, Name = "Group4", FormationDate = new DateTime(2021, 10, 6)},
            new Group {Id = 5, CourseId = 2, Name = "Group5", FormationDate = new DateTime(2018, 5, 29)}
        };

        private readonly List<Student> _studentsInMemoryDb = new()
        {
            new Student {Id = 1, GroupId = 1, FirstName = "Aa", LastName = "Zz", DateOfBirth = new DateTime(2018, 5, 23), FinalExamGpa = 4.63},
            new Student {Id = 2, GroupId = 1, FirstName = "Bb", LastName = "Yy", DateOfBirth = new DateTime(2019, 3, 12), FinalExamGpa = 3.05},
            new Student {Id = 3, GroupId = 1, FirstName = "Cc", LastName = "Xx", DateOfBirth = new DateTime(2019, 1, 5), FinalExamGpa = 4.15},
            new Student {Id = 4, GroupId = 1, FirstName = "Dd", LastName = "Vv", DateOfBirth = new DateTime(2021, 10, 6), FinalExamGpa = 2.87},
            new Student {Id = 5, GroupId = 1, FirstName = "Ee", LastName = "Uu", DateOfBirth = new DateTime(2018, 5, 29), FinalExamGpa = 4.53}
        };

        [Fact]
        public void VerifyCourseName_AddNewCourseWithDuplicateName_ReturnFalse()
        {
            const string name = "Course2";
            var duplicateVerifier = GetDuplicateCourseNameVerifier(name);

            bool result = duplicateVerifier.VerifyCourseName(0, name);

            result.Should().BeFalse();
        }

        [Fact]
        public void VerifyCourseName_AddNewCourseWithUniqueName_ReturnTrue()
        {
            const string name = "Course10";
            var duplicateVerifier = GetDuplicateCourseNameVerifier(name);

            bool result = duplicateVerifier.VerifyCourseName(0, name);

            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyCourseName_UpdateCourseWithoutNameChanging_ReturnTrue()
        {
            const string name = "Course2";
            var duplicateVerifier = GetDuplicateCourseNameVerifier(name);

            bool result = duplicateVerifier.VerifyCourseName(2, name);

            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyCourseName_UpdateCourseWithDuplicateName_ReturnFalse()
        {
            const string name = "Course2";
            var duplicateVerifier = GetDuplicateCourseNameVerifier(name);

            bool result = duplicateVerifier.VerifyCourseName(1, name);

            result.Should().BeFalse();
        }

        [Fact]
        public void VerifyGroupName_AddNewGroupWithDuplicateName_ReturnFalse()
        {
            const string name = "Group2";
            var duplicateVerifier = GetDuplicateGroupNameVerifier(name);

            bool result = duplicateVerifier.VerifyGroupName(0, name);

            result.Should().BeFalse();
        }

        [Fact]
        public void VerifyGroupName_AddNewGroupWithUniqueName_ReturnTrue()
        {
            const string name = "Group10";
            var duplicateVerifier = GetDuplicateGroupNameVerifier(name);

            bool result = duplicateVerifier.VerifyGroupName(0, name);

            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyGroupName_UpdateGroupWithoutNameChanging_ReturnTrue()
        {
            const string name = "Group2";
            var duplicateVerifier = GetDuplicateGroupNameVerifier(name);

            bool result = duplicateVerifier.VerifyGroupName(2, name);

            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyGroupName_UpdateGroupWithDuplicateName_ReturnFalse()
        {
            const string name = "Group2";
            var duplicateVerifier = GetDuplicateGroupNameVerifier(name);

            bool result = duplicateVerifier.VerifyGroupName(1, name);

            result.Should().BeFalse();
        }

        [Fact]
        public void VerifyStudent_AddNewStudentWithDuplicateAttributes_ReturnFalse()
        {
            const string firstName = "Bb";
            const string lastName = "Yy";
            var dateOfBirth = new DateTime(2019, 3, 12);
            var duplicateVerifier = GetDuplicateStudentVerifier(firstName, lastName, dateOfBirth);

            bool result = duplicateVerifier.VerifyStudent(0, firstName, lastName, dateOfBirth);

            result.Should().BeFalse();
        }

        [Fact]
        public void VerifyStudent_AddNewStudentWithUniqueAttributes_ReturnTrue()
        {
            const string firstName = "John";
            const string lastName = "Dow";
            var dateOfBirth = new DateTime(2016, 5, 12);
            var duplicateVerifier = GetDuplicateStudentVerifier(firstName, lastName, dateOfBirth);

            bool result = duplicateVerifier.VerifyStudent(0, firstName, lastName, dateOfBirth);

            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyStudent_UpdateStudentWithoutAttributesChanging_ReturnTrue()
        {
            const string firstName = "Bb";
            const string lastName = "Yy";
            var dateOfBirth = new DateTime(2019, 3, 12);
            var duplicateVerifier = GetDuplicateStudentVerifier(firstName, lastName, dateOfBirth);

            bool result = duplicateVerifier.VerifyStudent(2, firstName, lastName, dateOfBirth);

            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyStudent_UpdateStudentWithDuplicateAttributes_ReturnFalse()
        {
            const string firstName = "Bb";
            const string lastName = "Yy";
            var dateOfBirth = new DateTime(2019, 3, 12);
            var duplicateVerifier = GetDuplicateStudentVerifier(firstName, lastName, dateOfBirth);

            bool result = duplicateVerifier.VerifyStudent(1, firstName, lastName, dateOfBirth);

            result.Should().BeFalse();
        }

        private DuplicateVerifier GetDuplicateCourseNameVerifier(string courseName)
        {
            var courseRepository = new Mock<ICourseRepository>();
            courseRepository.Setup(x => x.Find(c => c.Name == courseName))
                .Returns(_coursesInMemoryDb.FindAll(c => c.Name == courseName));
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.Courses).Returns(courseRepository.Object);
            return new DuplicateVerifier(unitOfWork.Object);
        }

        private DuplicateVerifier GetDuplicateGroupNameVerifier(string groupName)
        {
            var groupRepository = new Mock<IGroupRepository>();
            groupRepository.Setup(x => x.Find(g => g.Name == groupName))
                .Returns(_groupsInMemoryDb.FindAll(c => c.Name == groupName));
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.Groups).Returns(groupRepository.Object);
            return new DuplicateVerifier(unitOfWork.Object);
        }

        private DuplicateVerifier GetDuplicateStudentVerifier(string firstName, string lastName, DateTime dateOfBirth)
        {
            var studentRepository = new Mock<IStudentRepository>();
            studentRepository.Setup(x => x.Find(s =>
                    s.FirstName == firstName && s.LastName == lastName && s.DateOfBirth == dateOfBirth))
                .Returns(_studentsInMemoryDb.FindAll(s =>
                    s.FirstName == firstName && s.LastName == lastName && s.DateOfBirth == dateOfBirth));
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.Students).Returns(studentRepository.Object);
            return new DuplicateVerifier(unitOfWork.Object);
        }
    }
}
