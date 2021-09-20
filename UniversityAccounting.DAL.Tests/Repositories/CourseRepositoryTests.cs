using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using UniversityAccounting.DAL.EF;
using UniversityAccounting.DAL.Entities;
using UniversityAccounting.DAL.Interfaces;
using UniversityAccounting.DAL.Repositories;

namespace UniversityAccounting.DAL.Tests.Repositories
{
    public class CourseRepositoryTests
    {
        [Fact]
        public void MyTestMethod()
        {
            var coursesInMemoryDb = (IQueryable<Course>) new List<Course>
            {
                new() {Id = 1, Name = "Course1"},
                new() {Id = 2, Name = "Course2"},
                new() {Id = 3, Name = "Course3"}
            };

            var context = new Mock<UniversityContext>();
            var dbSetMock = new Mock<DbSet<Course>>();
            context.Setup(x => x.Set<Course>()).Returns(dbSetMock.Object);
            dbSetMock.Setup(x => x.Where(It.IsAny<Expression<Func<Course, bool>>>())).Returns(coursesInMemoryDb);

            var repo = new CourseRepository(context.Object);
            int n = repo.SuitableCoursesCount("");

            context.Verify(x => x.Set<Course>());
            Assert.Equal(n, 3);
        }
    }
}
