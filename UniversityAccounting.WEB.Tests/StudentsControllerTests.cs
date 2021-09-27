using System;
using System.Collections.Generic;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Xunit;
using Moq;
using UniversityAccounting.DAL.Entities;
using UniversityAccounting.DAL.Interfaces;
using UniversityAccounting.WEB.Controllers;
using UniversityAccounting.WEB.Controllers.HelperClasses;
using UniversityAccounting.WEB.Models;

namespace UniversityAccounting.WEB.Tests
{
    public class StudentsControllerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<INotyfService> _notifMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ISortModel> _sortModelMock = new();
        private readonly Mock<IBreadcrumbNodeCreator> _nodesCreatorMock = new();
        private readonly Mock<IStringLocalizer<SharedResource>> _sharedLocalizerMock = new();
        private readonly Mock<IStringLocalizer<StudentsController>> _localizerMock = new();

        private readonly List<Student> _studentsInMemoryDb = new()
        {
            new Student
            {
                Id = 1, GroupId = 1, FirstName = "Aa", LastName = "Zz", DateOfBirth = new DateTime(2018, 5, 23),
                FinalExamGpa = 4.63, Status = 1
            },
            new Student
            {
                Id = 2, GroupId = 1, FirstName = "Bb", LastName = "Yy", DateOfBirth = new DateTime(2019, 3, 12),
                FinalExamGpa = 3.05, Status = 1
            },
            new Student
            {
                Id = 3, GroupId = 1, FirstName = "Cc", LastName = "Xx", DateOfBirth = new DateTime(2019, 1, 5),
                FinalExamGpa = 4.15, Status = 1
            },
            new Student
            {
                Id = 4, GroupId = 1, FirstName = "Dd", LastName = "Vv", DateOfBirth = new DateTime(2021, 10, 6),
                FinalExamGpa = 2.87, Status = 1
            },
            new Student
            {
                Id = 5, GroupId = 1, FirstName = "Ee", LastName = "Uu", DateOfBirth = new DateTime(2018, 5, 29),
                FinalExamGpa = 4.53, Status = 1
            },
            new Student
            {
                Id = 6, GroupId = 2, FirstName = "Ff", LastName = "Tt", DateOfBirth = new DateTime(2019, 8, 1),
                FinalExamGpa = 2.74, Status = 1
            },
            new Student
            {
                Id = 7, GroupId = 2, FirstName = "Gg", LastName = "Ss", DateOfBirth = new DateTime(2020, 4, 13),
                FinalExamGpa = 3.29, Status = 1
            },
            new Student
            {
                Id = 8, GroupId = 2, FirstName = "Hh", LastName = "Rr", DateOfBirth = new DateTime(2017, 11, 22),
                FinalExamGpa = 3.92, Status = 1
            },
            new Student
            {
                Id = 9, GroupId = 3, FirstName = "Ii", LastName = "Qq", DateOfBirth = new DateTime(2017, 7, 11),
                FinalExamGpa = 4.40, Status = 1
            },
            new Student
            {
                Id = 10, GroupId = 3, FirstName = "Jj", LastName = "Pp", DateOfBirth = new DateTime(2027, 6, 15),
                FinalExamGpa = 4.78, Status = 1
            }
        };

        [Fact]
        public void Create_ModelStateIsInvalid_ReturnsViewResultWithStudentViewModel()
        {
            var controller = new StudentsController(_unitOfWorkMock.Object, _notifMock.Object, _mapperMock.Object,
                _nodesCreatorMock.Object, _sortModelMock.Object, _sharedLocalizerMock.Object, _localizerMock.Object);
            controller.ModelState.AddModelError(string.Empty, "Some error");
            var studentModel = new StudentViewModel();

            var result = controller.Create(studentModel);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(studentModel, viewResult?.Model);
        }

        [Fact]
        public void Create_AddingToRepositoryError_ReturnsErrorView()
        {
            _unitOfWorkMock.Setup(u => u.Complete()).Throws<DbUpdateException>();
            var controller = new StudentsController(_unitOfWorkMock.Object, _notifMock.Object, _mapperMock.Object,
                _nodesCreatorMock.Object, _sortModelMock.Object, _sharedLocalizerMock.Object, _localizerMock.Object);
            var studentModel = new StudentViewModel();

            var result = controller.Create(studentModel);

            var viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewName.Should().Be("Error");
        }

        [Fact]
        public void Create_SuccessfulStudentAdding_ReturnsARedirectToIndexMethod()
        {
            var controller = new StudentsController(_unitOfWorkMock.Object, _notifMock.Object, _mapperMock.Object,
                _nodesCreatorMock.Object, _sortModelMock.Object, _sharedLocalizerMock.Object, _localizerMock.Object)
            {
                TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            };

            var studentModel = new StudentViewModel();
            var student = new Student();
            _mapperMock.Setup(m => m.Map<StudentViewModel, Student>(studentModel))
                .Returns(student);
            _unitOfWorkMock.Setup(u => u.Students.Add(It.IsAny<Student>()));
            var locString = new LocalizedString("message", "msg");
            _localizerMock.Setup(l => l[It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()])
                .Returns(locString);

            var result = controller.Create(studentModel);

            controller.TempData["message"].Should().Be("msg");
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ControllerName.Should().BeNull();
            redirectToActionResult.ActionName.Should().Be(nameof(StudentsController.Index));
            redirectToActionResult.RouteValues["groupId"].Should().Be(studentModel.GroupId);
            _unitOfWorkMock.Verify(u => u.Students.Add(student));
        }
    }
}
