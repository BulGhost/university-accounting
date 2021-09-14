using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Nodes;
using UniversityAccounting.DAL.Interfaces;
using UniversityAccounting.WEB.Models;
using AutoMapper;
using Microsoft.Extensions.Localization;
using UniversityAccounting.DAL.BusinessLogic;
using UniversityAccounting.DAL.Entities;
using UniversityAccounting.WEB.Models.HelperClasses;

namespace UniversityAccounting.WEB.Controllers
{
    public class StudentsController : BaseController
    {
        private const int StudentsPerPage = 10;
        private readonly IStringLocalizer<StudentsController> _localizer;

        public StudentsController(IUnitOfWork unitOfWork, INotyfService notyf, IMapper mapper,
            IStringLocalizer<SharedResource> sharedLocalizer, IStringLocalizer<StudentsController> localizer)
            :base(unitOfWork, notyf, sharedLocalizer, mapper)
        {
            _localizer = localizer;
        }

        public IActionResult Index(int groupId, int page = 1, string sortProperty = nameof(Student.LastName),
            SortOrder sortOrder = SortOrder.Ascending, string searchText = "")
        {
            var currentGroup = UnitOfWork.Groups.Get(groupId);
            if (currentGroup == null) return NotFound();

            ViewBag.Group = currentGroup;
            int totalStudents = UnitOfWork.Students.SuitableStudentsCount(s => s.GroupId == groupId, searchText);
            if (page < 1 || page > Math.Floor((double) totalStudents / StudentsPerPage) + 1)
                return RedirectToAction("Index", new {groupId, page = 1});

            var node1 = new MvcBreadcrumbNode("Index", "Courses", _localizer["Courses"]);
            var node2 = new MvcBreadcrumbNode("Index", "Groups", $"{currentGroup.Course.Name}")
                {RouteValues = new {courseId = currentGroup.CourseId}, Parent = node1};
            var node3 = new MvcBreadcrumbNode("Index", "Students", _localizer["GroupName", currentGroup.Name])
                {Parent = node2};
            ViewData["BreadcrumbNode"] = node3;

            if (TempData.ContainsKey("message")) Notyf.Success(TempData["message"].ToString());
            if (TempData.ContainsKey("error")) Notyf.Error(TempData["error"].ToString());

            var sortModel = new SortModel();
            sortModel.AddColumn(nameof(Student.FirstName));
            sortModel.AddColumn(nameof(Student.LastName), true);
            sortModel.AddColumn(nameof(Student.DateOfBirth));
            sortModel.AddColumn(nameof(Student.FinalExamGpa));
            sortModel.AddColumn(nameof(Student.Status));
            sortModel.ApplySort(sortProperty, sortOrder);

            var studentsOnPage = Mapper.Map<List<StudentViewModel>>(UnitOfWork.Students
                .GetRequiredStudents(s => s.GroupId == groupId, searchText, sortProperty,
                    page, StudentsPerPage, sortOrder));
            ViewBag.SearchText = searchText;

            return View(new StudentsIndexViewModel
            {
                Students = studentsOnPage,
                SortModel = sortModel,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = StudentsPerPage,
                    TotalItems = totalStudents
                }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSeveral(int?[] ids)
        {
            ICollection<Student> students = new List<Student>();
            foreach (int? id in ids)
            {
                if (id == null) continue;

                var student = UnitOfWork.Students.Get((int) id);
                if (student == null) return View("Error");

                students.Add(student);
            }

            try
            {
                UnitOfWork.Students.RemoveRange(students);
                UnitOfWork.Complete();
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData["message"] = _localizer["SeveralStudentsDeleted", students.Count].Value;
            return RedirectToAction("Index", new {groupId = students.First().GroupId});
        }

        public IActionResult Create(int? groupId)
        {
            if (groupId == null || groupId == 0) return NotFound();

            var currentGroup = UnitOfWork.Groups.Get((int) groupId);
            if (currentGroup == null) return NotFound();

            var node1 = new MvcBreadcrumbNode("Index", "Courses", _localizer["Courses"]);
            var node2 = new MvcBreadcrumbNode("Index", "Groups", $"{currentGroup.Course.Name}")
                {RouteValues = new {courseId = currentGroup.CourseId}, Parent = node1};
            var node3 = new MvcBreadcrumbNode("Index", "Students", _localizer["GroupName", currentGroup.Name])
                {RouteValues = new {groupId = currentGroup.Id}, Parent = node2};
            var node4 = new MvcBreadcrumbNode("Create", "Students", _localizer["NewStudent"]) { Parent = node3 };
            ViewData["BreadcrumbNode"] = node4;

            var student = new StudentViewModel {GroupId = (int) groupId};
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentViewModel studentModel)
        {
            if (!ModelState.IsValid) return View(studentModel);

            try
            {
                var student = Mapper.Map<StudentViewModel, Student>(studentModel);
                UnitOfWork.Students.Add(student);
                UnitOfWork.Complete();
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData["message"] = _localizer["StudentAdded", studentModel.FirstName, studentModel.LastName].Value;
            return RedirectToAction("Index", new { groupId = studentModel.GroupId });
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var student = UnitOfWork.Students.Get((int)id);
            if (student == null) return NotFound();

            var node1 = new MvcBreadcrumbNode("Index", "Courses", _localizer["Courses"]);
            var node2 = new MvcBreadcrumbNode("Index", "Groups", $"{student.Group.Course.Name}")
                {RouteValues = new {courseId = student.Group.CourseId}, Parent = node1};
            var node3 = new MvcBreadcrumbNode("Index", "Students", _localizer["GroupName", student.Group.Name])
                {RouteValues = new { groupId = student.Group.Id}, Parent = node2};
            var node4 = new MvcBreadcrumbNode("Create", "Students", _localizer["EditStudent"]) {Parent = node3};
            ViewData["BreadcrumbNode"] = node4;

            var studentModel = Mapper.Map<Student, StudentViewModel>(student);

            ViewBag.Groups = UnitOfWork.Groups.Find(g => g.CourseId == student.Group.CourseId);
            return View(studentModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StudentViewModel student)
        {
            if (!ModelState.IsValid)
            {
                var group = UnitOfWork.Groups.Get(student.GroupId);
                if (group == null) return NotFound();

                ViewBag.Groups = UnitOfWork.Groups.Find(g => g.CourseId == group.CourseId);
                return View(student);
            }

            try
            {
                var updStudent = UnitOfWork.Students.Get(student.Id);
                updStudent.FirstName = student.FirstName;
                updStudent.LastName = student.LastName;
                updStudent.DateOfBirth = student.DateOfBirth;
                updStudent.GroupId = student.GroupId;
                updStudent.FinalExamGpa = student.FinalExamGpa;
                updStudent.Status = student.Status;
                UnitOfWork.Complete();
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData["message"] = _localizer["StudentUpdated", student.FirstName, student.LastName].Value;
            return RedirectToAction("Index", new {groupId = student.GroupId});
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var student = UnitOfWork.Students.Get((int)id);
            if (student == null) return NotFound();

            var node1 = new MvcBreadcrumbNode("Index", "Courses", _localizer["Courses"]);
            var node2 = new MvcBreadcrumbNode("Index", "Groups", $"{student.Group.Course.Name}")
                {RouteValues = new {courseId = student.Group.CourseId }, Parent = node1};
            var node3 = new MvcBreadcrumbNode("Index", "Students", _localizer["GroupName", student.Group.Name])
                {RouteValues = new {groupId = student.Group.Id}, Parent = node2};
            var node4 = new MvcBreadcrumbNode("Create", "Students", _localizer["DeleteStudent"]) { Parent = node3 };
            ViewData["BreadcrumbNode"] = node4;

            var studentModel = Mapper.Map<Student, StudentViewModel>(student);

            return View(studentModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var student = UnitOfWork.Students.Get((int)id);
            if (student == null) return NotFound();

            try
            {
                UnitOfWork.Students.Remove(student);
                UnitOfWork.Complete();
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData["message"] = _localizer["StudentDeleted", student.FirstName, student.LastName].Value;
            return RedirectToAction("Index", new {groupId = student.GroupId});
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyStudent(int id, string firstName, string lastName, DateTime dateOfBirth)
        {
            return new DuplicateVerifier(UnitOfWork).VerifyStudent(id, firstName, lastName, dateOfBirth)
                ? Json(true)
                : Json(_localizer["StudentExists"].Value);
        }
    }
}
