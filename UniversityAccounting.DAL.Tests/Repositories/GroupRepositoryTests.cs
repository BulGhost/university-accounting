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
    public class GroupRepositoryTests
    {
        private readonly List<Group> _groupsInMemoryDb = new()
        {
            new Group {Id = 1, CourseId = 1, Name = "Group1", FormationDate = new DateTime(2018, 5, 23)},
            new Group {Id = 2, CourseId = 1, Name = "Group2", FormationDate = new DateTime(2019, 3, 12)},
            new Group {Id = 3, CourseId = 1, Name = "Group3", FormationDate = new DateTime(2019, 1, 5)},
            new Group {Id = 4, CourseId = 2, Name = "Group4", FormationDate = new DateTime(2021, 10, 6)},
            new Group {Id = 5, CourseId = 2, Name = "Group5", FormationDate = new DateTime(2018, 5, 29)},
            new Group {Id = 6, CourseId = 3, Name = "Group6", FormationDate = new DateTime(2019, 8, 1)},
            new Group {Id = 7, CourseId = 3, Name = "Group7", FormationDate = new DateTime(2020, 4, 13)}
        };

        private readonly GroupRepository _repo;

        public static IEnumerable<object[]> SortingTestData =>
            new List<object[]>
            {
                new object[]
                {
                    1,
                    "Name",
                    SortOrder.Ascending,
                    new List<Group>
                    {
                        new() {Id = 1, CourseId = 1, Name = "Group1", FormationDate = new DateTime(2018, 5, 23)},
                        new() {Id = 2, CourseId = 1, Name = "Group2", FormationDate = new DateTime(2019, 3, 12)},
                        new() {Id = 3, CourseId = 1, Name = "Group3", FormationDate = new DateTime(2019, 1, 5)}
                    }
                },
                new object[]
                {
                    2,
                    "Name",
                    SortOrder.Descending,
                    new List<Group>
                    {
                        new() {Id = 5, CourseId = 2, Name = "Group5", FormationDate = new DateTime(2018, 5, 29)},
                        new() {Id = 4, CourseId = 2, Name = "Group4", FormationDate = new DateTime(2021, 10, 6)}
                    }
                },
                new object[]
                {
                    3,
                    "FormationDate",
                    SortOrder.Descending,
                    new List<Group>
                    {
                        new() {Id = 7, CourseId = 3, Name = "Group7", FormationDate = new DateTime(2020, 4, 13)},
                        new() {Id = 6, CourseId = 3, Name = "Group6", FormationDate = new DateTime(2019, 8, 1)}
                    }
                }
            };

        public static IEnumerable<object[]> PaginationTestData =>
            new List<object[]>
            {
                new object[]
                {
                    2,
                    1,
                    new List<Group>
                    {
                        new() {Id = 2, CourseId = 1, Name = "Group2", FormationDate = new DateTime(2019, 3, 12)}
                    }
                },
                new object[]
                {
                    2,
                    2,
                    new List<Group>
                    {
                        new() {Id = 3, CourseId = 1, Name = "Group3", FormationDate = new DateTime(2019, 1, 5)}
                    }
                },
                new object[]
                {
                    1,
                    5,
                    new List<Group>
                    {
                        new() {Id = 1, CourseId = 1, Name = "Group1", FormationDate = new DateTime(2018, 5, 23)},
                        new() {Id = 2, CourseId = 1, Name = "Group2", FormationDate = new DateTime(2019, 3, 12)},
                        new() {Id = 3, CourseId = 1, Name = "Group3", FormationDate = new DateTime(2019, 1, 5)}
                    }
                },
                new object[]
                {
                    3,
                    5,
                    new List<Group>()
                }
            };

        public GroupRepositoryTests()
        {
            var dbSetMock = new Mock<DbSet<Group>>();
            dbSetMock.As<IQueryable<Group>>().Setup(x => x.Provider).Returns(_groupsInMemoryDb.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Group>>().Setup(x => x.Expression).Returns(_groupsInMemoryDb.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Group>>().Setup(x => x.ElementType).Returns(_groupsInMemoryDb.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Group>>().Setup(x => x.GetEnumerator()).Returns(_groupsInMemoryDb.AsQueryable().GetEnumerator());

            var context = new Mock<UniversityContext>();
            context.Setup(x => x.Set<Group>()).Returns(dbSetMock.Object);
            _repo = new GroupRepository(context.Object);
        }

        [InlineData(1, "", 3)]
        [InlineData(3, null, 2)]
        [Theory]
        public void SuitableGroupsCount_NoFilter_TotalGroupsCount(int courseId, string searchText, int expectedCount)
        {
            int result = _repo.SuitableGroupsCount(g => g.CourseId == courseId, searchText);

            Assert.Equal(expectedCount, result);
        }

        [InlineData(1, "Group3", 1)]
        [InlineData(1, "2019", 2)]
        [InlineData(2, "6.10.2021", 1)]
        [Theory]
        public void SuitableGroupsCount_WithFilter_RequiredGroupsCount(int courseId, string searchText, int expectedCount)
        {
            int result = _repo.SuitableGroupsCount(g => g.CourseId == courseId, searchText);

            Assert.Equal(expectedCount, result);
        }

        [Fact]
        public void SuitableGroupsCount_WithFormationDateFilter_RequiredGroupsCount()
        {
            string searchText = new DateTime(2021, 10, 6).ToShortDateString();

            int result = _repo.SuitableGroupsCount(g => g.CourseId == 2, searchText);

            Assert.Equal(1, result);
        }

        [InlineData(null)]
        [InlineData("")]
        [InlineData("2kz6")]
        [Theory]
        public void GetRequiredCourses_InvalidSortProperty_ArgumentException(string sortProperty)
        {
            Assert.ThrowsAny<ArgumentException>(() => _repo.GetRequiredGroups(
                g => g.CourseId == 1, "", sortProperty, 1, 3));
        }

        [MemberData(nameof(SortingTestData))]
        [Theory]
        public void GetRequiredGroups_ValidSortProperty_OrderedGroupsList(int courseId, string sortProperty,
            SortOrder sortOrder, List<Group> expectedGroups)
        {
            var result = _repo.GetRequiredGroups(g => g.CourseId == courseId,
                "", sortProperty, 1, 3, sortOrder: sortOrder);

            result.Should().Equal(expectedGroups);
        }

        [MemberData(nameof(PaginationTestData))]
        [Theory]
        public void GetRequiredGroups_Pagination_RequiredGroupsList(int pageIndex, int pageSize, List<Group> expectedGroups)
        {
            var result = _repo.GetRequiredGroups(g => g.CourseId == 1, "", "Name", pageIndex, pageSize);

            result.Should().Equal(expectedGroups);
        }
    }
}
