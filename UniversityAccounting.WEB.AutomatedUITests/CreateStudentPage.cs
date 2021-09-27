using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace UniversityAccounting.WEB.AutomatedUITests
{
    public class CreateStudentPage
    {
        private readonly IWebDriver _driver;
        private readonly string _url;
        private IWebElement FirstNameElement => _driver.FindElement(By.Name("FirstName"));
        private IWebElement LastNameElement => _driver.FindElement(By.Name("LastName"));
        private IWebElement DateOfBirthElement => _driver.FindElement(By.Name("DateOfBirth"));
        private IWebElement GpaElement => _driver.FindElement(By.Name("FinalExamGpa"));
        private IWebElement StatusElement => _driver.FindElement(By.Name("Status"));
        private IWebElement CreateElement => _driver.FindElement(By.CssSelector("input[type='submit']"));
        private IWebElement BackElement => _driver.FindElement(By.Id("backButton"));
        private IWebElement AddNewElement => _driver.FindElement(By.Id("add-new"));
        public string Title => _driver.Title;
        public string Source => _driver.PageSource;
        public string ValidationErrorMessage => _driver.FindElement(By.Id("validation-summary")).Text;
        public string AddNewText => AddNewElement.Text;

        public CreateStudentPage(IWebDriver driver, int groupId)
        {
            _driver = driver;
            _url = $"https://localhost:5001/Students/Create?groupId={groupId}";
        }

        public void Navigate() => _driver.Navigate()
            .GoToUrl(_url);

        public bool FirstNameDisplayed() => FirstNameElement.Displayed;
        public void PopulateFirstName(string firstName) => FirstNameElement.SendKeys(firstName);
        public void FirstNameClick() => FirstNameElement.Click();
        public bool LastNameDisplayed() => LastNameElement.Displayed;
        public void PopulateLastName(string lastName) => LastNameElement.SendKeys(lastName);
        public bool DateOfBirthNameDisplayed() => DateOfBirthElement.Displayed;
        public void ClearDateOfBirth() => DateOfBirthElement.Clear();
        public void PopulateDateOfBirth(DateTime date) => DateOfBirthElement.SendKeys(date.ToShortDateString());
        public void PopulateGpa(string grade) => GpaElement.SendKeys(grade);

        public void SelectStatus(int status)
        {
            var selectElement = new SelectElement(StatusElement);
            selectElement.SelectByValue(status.ToString());
        }

        public bool CreateDisplayed() => CreateElement.Displayed;
        public void SubmitCreate() => CreateElement.Submit();
        public bool BackDisplayed() => BackElement.Displayed;
        public bool AddNewDisplayed() => AddNewElement.Displayed;
    }
}
