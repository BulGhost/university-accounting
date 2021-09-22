using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using UniversityAccounting.DAL.Entities;
using Xunit;

namespace UniversityAccounting.WEB.AutomatedUITests
{
    public class StudentCreateAutomatedUiTests : IDisposable, IClassFixture<ContextFixture>
    {
        private readonly IWebDriver _driver;
        private readonly ContextFixture _fixture;
        private readonly CreateStudentPage _page;

        public StudentCreateAutomatedUiTests(ContextFixture fixture)
        {
            _driver = new ChromeDriver();
            _fixture = fixture;
            _page = new CreateStudentPage(_driver, _fixture.TestGroupId);
            _page.Navigate();
        }

        [Fact]
        public void Create_WhenExecuted_ReturnsCreateView()
        {
            Assert.Equal(Resources.Views.Shared._Layout.UniversityAccounting
                         + " - " + Resources.Views.Students.Create.AddStudent, _page.Title);
            Assert.Contains(Resources.Views.Students.Create.AddStudent, _page.Source);
            Assert.True(_page.FirstNameDisplayed());
            Assert.True(_page.LastNameDisplayed());
            Assert.True(_page.DateOfBirthNameDisplayed());
            Assert.True(_page.CreateDisplayed());
            Assert.True(_page.BackDisplayed());
        }

        [Fact]
        public void Create_FirstNameNotEntered_ReturnsErrorMessage()
        {
            _page.PopulateLastName("Black");
            _page.SelectStatus(1);
            _page.SubmitCreate();

            Assert.Equal(Resources.Models.StudentViewModel.FirstNameRequired, _page.ValidationErrorMessage);
        }

        [Fact]
        public void Create_EnteredGPAIsInvalid_ReturnsErrorMessage()
        {
            _page.PopulateFirstName("Joe");
            _page.PopulateLastName("Black");
            _page.PopulateGpa("abc");
            _page.SelectStatus(1);
            _page.SubmitCreate();

            string expectedErrorMessage = string.Format(Resources.Startup.ValueMustBeANumber, Resources.Models.StudentViewModel.Gpa);
            Assert.Equal(expectedErrorMessage, _page.ValidationErrorMessage);
        }

        [Fact]
        public void Create_EnteredGPAIsInInvalidRange_ReturnsErrorMessage()
        {
            _page.PopulateFirstName("Joe");
            _page.PopulateLastName("Black");
            _page.PopulateGpa("0");
            _page.SelectStatus(1);
            _page.SubmitCreate();

            Assert.Equal(Resources.Models.StudentViewModel.GpaRangeError, _page.ValidationErrorMessage);
        }

        [Fact]
        public void Create_StatusIsNotSelected_ReturnsErrorMessage()
        {
            _page.PopulateFirstName("Joe");
            _page.PopulateLastName("Black");
            _page.SubmitCreate();

            Assert.Equal(Resources.Models.StudentViewModel.ChooseStatus, _page.ValidationErrorMessage);
        }

        [Fact]
        public void Create_WhenSuccessfullyExecuted_ReturnsIndexViewWithNewEmployee()
        {
            var student = new Student
            {
                FirstName = "Joe1", LastName = "Black2", FinalExamGpa = 4,
                DateOfBirth = new DateTime(1995, 5, 5), Status = 1
            };

            _page.PopulateFirstName(student.FirstName);
            _page.PopulateLastName(student.LastName);
            _page.ClearDateOfBirth();
            _page.PopulateDateOfBirth(student.DateOfBirth);
            _page.PopulateGpa(student.FinalExamGpa.ToString());
            _page.SelectStatus(student.Status);
            _page.FirstNameClick();
            _page.SubmitCreate();

            _fixture.TestStudent = student;
            string title = Resources.Views.Shared._Layout.UniversityAccounting + " - " +
                           string.Format(Resources.Views.Students.Index.GroupStudents, _fixture.TestGroupName);
            Assert.Equal(title, _page.Title);
            Assert.Contains(string.Format(Resources.Views.Students.Index.GroupStudents, _fixture.TestGroupName),
                _page.Source);
            Assert.True(_page.AddNewDisplayed());
            Assert.Equal(Resources.Views.Students.Index.AddNewStudent, _page.AddNewText);
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
