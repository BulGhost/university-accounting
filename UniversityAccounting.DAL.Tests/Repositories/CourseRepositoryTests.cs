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
    public class CourseRepositoryTests
    {
        private readonly List<Course> _coursesInMemoryDb = new()
        {
            new Course {Id = 1, Name = "Course1", Description = "first"},
            new Course {Id = 2, Name = "Course2", Description = "second foo"},
            new Course {Id = 3, Name = "Course3", Description = "third" },
            new Course {Id = 4, Name = "Course4", Description = "fourth foo"},
            new Course {Id = 5, Name = "Course5", Description = "fifth" },
            new Course {Id = 6, Name = "Course6", Description = "sixth foo"},
            new Course {Id = 7, Name = "Course7", Description = ""}
        };

        private readonly CourseRepository _repo;

        public static IEnumerable<object[]> SortingTestData =>
            new List<object[]>
            {
                new object[]
                {
                    "Name",
                    SortOrder.Ascending,
                    new List<Course>
                    {
                        new() {Id = 1, Name = "Course1", Description = "first"},
                        new() {Id = 2, Name = "Course2", Description = "second foo"},
                        new() {Id = 3, Name = "Course3", Description = "third"}
                    }
                },
                new object[]
                {
                    "Name",
                    SortOrder.Descending,
                    new List<Course>
                    {
                        new() {Id = 7, Name = "Course7", Description = ""},
                        new() {Id = 6, Name = "Course6", Description = "sixth foo"},
                        new() {Id = 5, Name = "Course5", Description = "fifth"}
                    }
                },
                new object[]
                {
                    "Description",
                    SortOrder.Descending,
                    new List<Course>
                    {
                        new() {Id = 3, Name = "Course3", Description = "third"},
                        new() {Id = 6, Name = "Course6", Description = "sixth foo"},
                        new() {Id = 2, Name = "Course2", Description = "second foo"}
                    }
                }
            };

        public static IEnumerable<object[]> PaginationTestData =>
            new List<object[]>
            {
                new object[]
                {
                    3,
                    2,
                    new List<Course>
                    {
                        new() {Id = 5, Name = "Course5", Description = "fifth"},
                        new() {Id = 6, Name = "Course6", Description = "sixth foo"}
                    }
                },
                new object[]
                {
                    2,
                    4,
                    new List<Course>
                    {
                        new() {Id = 5, Name = "Course5", Description = "fifth"},
                        new() {Id = 6, Name = "Course6", Description = "sixth foo"},
                        new() {Id = 7, Name = "Course7", Description = ""}
                    }
                },
                new object[]
                {
                    1,
                    10,
                    new List<Course>
                    {
                        new() {Id = 1, Name = "Course1", Description = "first"},
                        new() {Id = 2, Name = "Course2", Description = "second foo"},
                        new() {Id = 3, Name = "Course3", Description = "third"},
                        new() {Id = 4, Name = "Course4", Description = "fourth foo"},
                        new() {Id = 5, Name = "Course5", Description = "fifth"},
                        new() {Id = 6, Name = "Course6", Description = "sixth foo"},
                        new() {Id = 7, Name = "Course7", Description = ""}
                    }
                },
                new object[]
                {
                    3,
                    10,
                    new List<Course>()
                }
            };

        public CourseRepositoryTests()
        {
            var dbSetMock = new Mock<DbSet<Course>>();
            dbSetMock.As<IQueryable<Course>>().Setup(x => x.Provider).Returns(_coursesInMemoryDb.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Course>>().Setup(x => x.Expression).Returns(_coursesInMemoryDb.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Course>>().Setup(x => x.ElementType).Returns(_coursesInMemoryDb.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Course>>().Setup(x => x.GetEnumerator()).Returns(_coursesInMemoryDb.AsQueryable().GetEnumerator());

            var context = new Mock<UniversityContext>();
            context.Setup(x => x.Set<Course>()).Returns(dbSetMock.Object);
            _repo = new CourseRepository(context.Object);
        }

        [InlineData("")]
        [InlineData(null)]
        [Theory]
        public void SuitableCoursesCount_NoFilter_TotalCoursesCount(string searchText)
        {
            int result = _repo.SuitableCoursesCount(searchText);

            Assert.Equal(_coursesInMemoryDb.Count, result);
        }

        [InlineData("Course4", 1)]
        [InlineData("foo", 3)]
        [InlineData("tenth", 0)]
        [Theory]
        public void SuitableCoursesCount_WithFilter_RequiredCoursesCount(string searchText, int count)
        {
            int result = _repo.SuitableCoursesCount(searchText);

            Assert.Equal(count, result);
        }

        [InlineData(null)]
        [InlineData("")]
        [InlineData("2kz6")]
        [Theory]
        public void GetRequiredCourses_InvalidSortProperty_ArgumentException(string sortProperty)
        {
            Assert.ThrowsAny<ArgumentException>(() => _repo.GetRequiredCourses("", sortProperty, 1, 3));
        }

        [MemberData(nameof(SortingTestData))]
        [Theory]
        public void GetRequiredCourses_ValidSortProperty_OrderedCoursesList(string sortProperty,
            SortOrder sortOrder, List<Course> expectedCourses)
        {
            var courses = _repo.GetRequiredCourses("", sortProperty, 1, 3, sortOrder: sortOrder);

            courses.Should().Equal(expectedCourses);
        }

        [MemberData(nameof(PaginationTestData))]
        [Theory]
        public void GetRequiredCourses_Pagination_RequiredCoursesList(int pageIndex, int pageSize, List<Course> expectedCourses)
        {
            var courses = _repo.GetRequiredCourses("", "Name", pageIndex, pageSize);

            courses.Should().Equal(expectedCourses);
        }
    }
}