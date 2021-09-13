using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using UniversityAccounting.DAL.Entities;
using UniversityAccounting.DAL.Interfaces;
using Xunit;

namespace UniversityAccounting.DAL.Tests.Repositories
{
    public class BaseRepositoryTests
    {
        [Fact]
        public void MyTestMethod()
        {
            var coursesInMemoryDb = new List<Course>
            {
                new() {Id = 1, Name = "Course1"},
                new() {Id = 2, Name = "Course2"},
                new() {Id = 3, Name = "Course3"}
            };

            var repository = new Mock<IRepository<Course>>();
        }
    }
}