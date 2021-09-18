using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using FluentAssertions;
using UniversityAccounting.DAL.EF;
using UniversityAccounting.DAL.Entities;
using UniversityAccounting.DAL.Interfaces;
using UniversityAccounting.DAL.Repositories;

namespace UniversityAccounting.DAL.Tests.Repositories
{
    public class StudentRepositoryTests
    {
        private readonly List<Student> _studentsInMemoryDb = new()
        {
            new Student {Id = 1, GroupId = 1, FirstName = "Aa", LastName = "Zz", DateOfBirth = new DateTime(2018, 5, 23), FinalExamGpa = 4.63},
            new Student {Id = 2, GroupId = 1, FirstName = "Bb", LastName = "Yy", DateOfBirth = new DateTime(2019, 3, 12), FinalExamGpa = 3.05},
            new Student {Id = 3, GroupId = 1, FirstName = "Cc", LastName = "Xx", DateOfBirth = new DateTime(2019, 1, 5), FinalExamGpa = 4.15},
            new Student {Id = 4, GroupId = 1, FirstName = "Dd", LastName = "Vv", DateOfBirth = new DateTime(2021, 10, 6), FinalExamGpa = 2.87},
            new Student {Id = 5, GroupId = 1, FirstName = "Ee", LastName = "Uu", DateOfBirth = new DateTime(2018, 5, 29), FinalExamGpa = 4.53},
            new Student {Id = 6, GroupId = 2, FirstName = "Ff", LastName = "Tt", DateOfBirth = new DateTime(2019, 8, 1), FinalExamGpa = 2.74},
            new Student {Id = 7, GroupId = 2, FirstName = "Gg", LastName = "Ss", DateOfBirth = new DateTime(2020, 4, 13), FinalExamGpa = 3.29},
            new Student {Id = 8, GroupId = 2, FirstName = "Hh", LastName = "Rr", DateOfBirth = new DateTime(2017, 11, 22), FinalExamGpa = 3.92},
            new Student {Id = 9, GroupId = 3, FirstName = "Ii", LastName = "Qq", DateOfBirth = new DateTime(2017, 7, 11), FinalExamGpa = 4.40},
            new Student {Id = 10, GroupId = 3, FirstName = "Jj", LastName = "Pp", DateOfBirth = new DateTime(2027, 6, 15), FinalExamGpa = 4.78}
        };

        private readonly StudentRepository _repo;

        public static IEnumerable<object[]> SortingTestData =>
            new List<object[]>
            {
                new object[]
                {
                    1,
                    "FirstName",
                    SortOrder.Ascending,
                    new List<Student>
                    {
                        new() {Id = 1, GroupId = 1, FirstName = "Aa", LastName = "Zz", DateOfBirth = new DateTime(2018, 5, 23), FinalExamGpa = 4.63},
                        new() {Id = 2, GroupId = 1, FirstName = "Bb", LastName = "Yy", DateOfBirth = new DateTime(2019, 3, 12), FinalExamGpa = 3.05},
                        new() {Id = 3, GroupId = 1, FirstName = "Cc", LastName = "Xx", DateOfBirth = new DateTime(2019, 1, 5), FinalExamGpa = 4.15}
                    }
                },
                new object[]
                {
                    2,
                    "LastName",
                    SortOrder.Descending,
                    new List<Student>
                    {
                        new() {Id = 6, GroupId = 2, FirstName = "Ff", LastName = "Tt", DateOfBirth = new DateTime(2019, 8, 1), FinalExamGpa = 2.74},
                        new() {Id = 7, GroupId = 2, FirstName = "Gg", LastName = "Ss", DateOfBirth = new DateTime(2020, 4, 13), FinalExamGpa = 3.29},
                        new() {Id = 8, GroupId = 2, FirstName = "Hh", LastName = "Rr", DateOfBirth = new DateTime(2017, 11, 22), FinalExamGpa = 3.92}
                    }
                },
                new object[]
                {
                    3,
                    "DateOfBirth",
                    SortOrder.Descending,
                    new List<Student>
                    {
                        new() {Id = 10, GroupId = 3, FirstName = "Jj", LastName = "Pp", DateOfBirth = new DateTime(2027, 6, 15), FinalExamGpa = 4.78},
                        new() {Id = 9, GroupId = 3, FirstName = "Ii", LastName = "Qq", DateOfBirth = new DateTime(2017, 7, 11), FinalExamGpa = 4.40}
                    }
                },
                new object[]
                {
                    1,
                    "FinalExamGpa",
                    SortOrder.Descending,
                    new List<Student>
                    {
                        new() {Id = 1, GroupId = 1, FirstName = "Aa", LastName = "Zz", DateOfBirth = new DateTime(2018, 5, 23), FinalExamGpa = 4.63},
                        new() {Id = 5, GroupId = 1, FirstName = "Ee", LastName = "Uu", DateOfBirth = new DateTime(2018, 5, 29), FinalExamGpa = 4.53},
                        new() {Id = 3, GroupId = 1, FirstName = "Cc", LastName = "Xx", DateOfBirth = new DateTime(2019, 1, 5), FinalExamGpa = 4.15}
                    }
                }
            };

        public static IEnumerable<object[]> PaginationTestData =>
            new List<object[]>
            {
                new object[]
                {
                    1,
                    2,
                    new List<Student>
                    {
                        new() {Id = 1, GroupId = 1, FirstName = "Aa", LastName = "Zz", DateOfBirth = new DateTime(2018, 5, 23), FinalExamGpa = 4.63},
                        new() {Id = 2, GroupId = 1, FirstName = "Bb", LastName = "Yy", DateOfBirth = new DateTime(2019, 3, 12), FinalExamGpa = 3.05}
                    }
                },
                new object[]
                {
                    3,
                    2,
                    new List<Student>
                    {
                        new() {Id = 5, GroupId = 1, FirstName = "Ee", LastName = "Uu", DateOfBirth = new DateTime(2018, 5, 29), FinalExamGpa = 4.53}
                    }
                },
                new object[]
                {
                    1,
                    10,
                    new List<Student>
                    {
                        new() {Id = 1, GroupId = 1, FirstName = "Aa", LastName = "Zz", DateOfBirth = new DateTime(2018, 5, 23), FinalExamGpa = 4.63},
                        new() {Id = 2, GroupId = 1, FirstName = "Bb", LastName = "Yy", DateOfBirth = new DateTime(2019, 3, 12), FinalExamGpa = 3.05},
                        new() {Id = 3, GroupId = 1, FirstName = "Cc", LastName = "Xx", DateOfBirth = new DateTime(2019, 1, 5), FinalExamGpa = 4.15},
                        new() {Id = 4, GroupId = 1, FirstName = "Dd", LastName = "Vv", DateOfBirth = new DateTime(2021, 10, 6), FinalExamGpa = 2.87},
                        new() {Id = 5, GroupId = 1, FirstName = "Ee", LastName = "Uu", DateOfBirth = new DateTime(2018, 5, 29), FinalExamGpa = 4.53}
                    }
                },
                new object[]
                {
                    3,
                    10,
                    new List<Student>()
                }
            };

        public StudentRepositoryTests()
        {
            var dbSetMock = new Mock<DbSet<Student>>();
            dbSetMock.As<IQueryable<Student>>().Setup(x => x.Provider)
                .Returns(_studentsInMemoryDb.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Student>>().Setup(x => x.Expression)
                .Returns(_studentsInMemoryDb.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Student>>().Setup(x => x.ElementType)
                .Returns(_studentsInMemoryDb.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Student>>().Setup(x => x.GetEnumerator())
                .Returns(_studentsInMemoryDb.AsQueryable().GetEnumerator());

            var context = new Mock<UniversityContext>();
            context.Setup(x => x.Set<Student>()).Returns(dbSetMock.Object);
            _repo = new StudentRepository(context.Object);
        }

        [InlineData(1, "", 5)]
        [InlineData(3, null, 2)]
        [Theory]
        public void SuitableStudentsCount_NoFilter_TotalStudentsCount(int groupId, string searchText, int expectedCount)
        {
            int result = _repo.SuitableStudentsCount(s => s.GroupId == groupId, searchText);

            Assert.Equal(expectedCount, result);
        }

        [InlineData(1, "Cc", 1)]
        [InlineData(2, "Cc", 0)]
        [InlineData(1, "2019", 2)]
        [InlineData(2, "22.11.2017", 1)]
        [Theory]
        public void SuitableStudentsCount_WithFilter_RequiredStudentsCount(int groupId, string searchText,
            int expectedCount)
        {
            int result = _repo.SuitableStudentsCount(s => s.GroupId == groupId, searchText);

            Assert.Equal(expectedCount, result);
        }

        [InlineData(null)]
        [InlineData("")]
        [InlineData("2kz6")]
        [Theory]
        public void GetRequiredStudents_InvalidSortProperty_ArgumentException(string sortProperty)
        {
            Assert.ThrowsAny<ArgumentException>(() => _repo.GetRequiredStudents(
                s => s.GroupId == 1, "", sortProperty, 1, 3));
        }

        [MemberData(nameof(SortingTestData))]
        [Theory]
        public void GetRequiredStudents_ValidSortProperty_OrderedStudentsList(int groupId, string sortProperty,
            SortOrder sortOrder, List<Student> expectedStudents)
        {
            var result = _repo.GetRequiredStudents(s => s.GroupId == groupId,
                "", sortProperty, 1, 3, sortOrder: sortOrder);

            result.Should().Equal(expectedStudents);
        }

        [MemberData(nameof(PaginationTestData))]
        [Theory]
        public void GetRequiredStudents_Pagination_RequiredStudentsList(int pageIndex, int pageSize,
            List<Student> expectedStudents)
        {
            var result = _repo.GetRequiredStudents(s => s.GroupId == 1,
                "", "FirstName", pageIndex, pageSize);

            result.Should().Equal(expectedStudents);
        }
    }
}