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
    public class StudentsController : Controller
    {
        private const int StudentsPerPage = 10;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService _notyf;
        private readonly IStringLocalizer<StudentsController> _localizer;
        private readonly IMapper _mapper;

        public StudentsController(IUnitOfWork unitOfWork, INotyfService notyf, IStringLocalizer<StudentsController> localizer, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _notyf = notyf;
            _localizer = localizer;
            _mapper = mapper;
        }

        public IActionResult Index(int groupId, int page = 1)
        {
            var currentGroup = _unitOfWork.Groups.Get(groupId);
            if (currentGroup == null) return NotFound();

            ViewBag.Group = currentGroup;
            var node1 = new MvcBreadcrumbNode("Index", "Courses", _localizer["Courses"]);
            var node2 = new MvcBreadcrumbNode("Index", "Groups", $"{currentGroup.Course.Name}")
                {RouteValues = new {courseId = currentGroup.CourseId}, Parent = node1};
            var node3 = new MvcBreadcrumbNode("Index", "Students", _localizer["GroupName", currentGroup.Name])
                {Parent = node2};
            ViewData["BreadcrumbNode"] = node3;
            if (TempData.ContainsKey("message")) _notyf.Success(TempData["message"].ToString());

            var studentsOnPage = _mapper.Map<List<StudentViewModel>>(_unitOfWork.Students
                .GetPart(s => s.GroupId == groupId, nameof(Student.Id), page, StudentsPerPage));

            return View(new StudentsIndexViewModel
            {
                Students = studentsOnPage,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = StudentsPerPage,
                    TotalItems = _unitOfWork.Students.Find(s => s.GroupId == groupId).Count()
                }
            });
        }

        public IActionResult Create(int? groupId)
        {
            if (groupId == null || groupId == 0) return NotFound();

            var currentGroup = _unitOfWork.Groups.Get((int) groupId);
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
                var student = _mapper.Map<StudentViewModel, Student>(studentModel);
                _unitOfWork.Students.Add(student);
                _unitOfWork.Complete();
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

            var student = _unitOfWork.Students.Get((int)id);
            if (student == null) return NotFound();

            var node1 = new MvcBreadcrumbNode("Index", "Courses", _localizer["Courses"]);
            var node2 = new MvcBreadcrumbNode("Index", "Groups", $"{student.Group.Course.Name}")
                {RouteValues = new {courseId = student.Group.CourseId}, Parent = node1};
            var node3 = new MvcBreadcrumbNode("Index", "Students", _localizer["GroupName", student.Group.Name])
                {RouteValues = new { groupId = student.Group.Id}, Parent = node2};
            var node4 = new MvcBreadcrumbNode("Create", "Students", _localizer["EditStudent"]) {Parent = node3};
            ViewData["BreadcrumbNode"] = node4;

            var studentModel = _mapper.Map<Student, StudentViewModel>(student);

            ViewBag.Groups = _unitOfWork.Groups.Find(g => g.CourseId == student.Group.CourseId);
            return View(studentModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StudentViewModel student)
        {
            if (!ModelState.IsValid)
            {
                var group = _unitOfWork.Groups.Get(student.GroupId);
                if (group == null) return NotFound();

                ViewBag.Groups = _unitOfWork.Groups.Find(g => g.CourseId == group.CourseId);
                return View(student);
            }

            try
            {
                var updStudent = _unitOfWork.Students.Get(student.Id);
                updStudent.FirstName = student.FirstName;
                updStudent.LastName = student.LastName;
                updStudent.DateOfBirth = student.DateOfBirth;
                updStudent.GroupId = student.GroupId;
                updStudent.FinalExamGpa = student.FinalExamGpa;
                updStudent.Status = student.Status;
                _unitOfWork.Complete();
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

            var student = _unitOfWork.Students.Get((int)id);
            if (student == null) return NotFound();

            var node1 = new MvcBreadcrumbNode("Index", "Courses", _localizer["Courses"]);
            var node2 = new MvcBreadcrumbNode("Index", "Groups", $"{student.Group.Course.Name}")
                {RouteValues = new {courseId = student.Group.CourseId }, Parent = node1};
            var node3 = new MvcBreadcrumbNode("Index", "Students", _localizer["GroupName", student.Group.Name])
                {RouteValues = new {groupId = student.Group.Id}, Parent = node2};
            var node4 = new MvcBreadcrumbNode("Create", "Students", _localizer["DeleteStudent"]) { Parent = node3 };
            ViewData["BreadcrumbNode"] = node4;

            var studentModel = _mapper.Map<Student, StudentViewModel>(student);

            return View(studentModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var student = _unitOfWork.Students.Get((int)id);
            if (student == null) return NotFound();

            try
            {
                _unitOfWork.Students.Remove(student);
                _unitOfWork.Complete();
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
            return new DuplicateVerifier().VerifyStudent(id, firstName, lastName, dateOfBirth)
                ? Json(true)
                : Json(_localizer["StudentExists"].Value);
        }
    }
}
