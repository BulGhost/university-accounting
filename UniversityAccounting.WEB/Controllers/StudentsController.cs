using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Nodes;
using UniversityAccounting.DAL.Interfaces;
using UniversityAccounting.WEB.Models;
using AutoMapper;
using UniversityAccounting.DAL.BusinessLogic;
using UniversityAccounting.DAL.Entities;

namespace UniversityAccounting.WEB.Controllers
{
    public class StudentsController : Controller
    {
        private const int StudentsPerPage = 10;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService _notyf;

        public StudentsController(IUnitOfWork unitOfWork, INotyfService notyf)
        {
            _unitOfWork = unitOfWork;
            _notyf = notyf;
        }

        public IActionResult Index(int groupId, int page = 1)
        {
            var currentGroup = _unitOfWork.Groups.Get(groupId);
            if (currentGroup == null) return NotFound();

            ViewBag.Group = currentGroup;
            var parentNode = new MvcBreadcrumbNode("Index", "Groups", $"{currentGroup.Course.Name}") { RouteValues = new { courseId = currentGroup.CourseId } };
            var childNode = new MvcBreadcrumbNode("Index", "Students", $"{currentGroup.Name} group") { Parent = parentNode };
            ViewData["BreadcrumbNode"] = childNode;
            if (TempData.ContainsKey("message")) _notyf.Success(TempData["message"].ToString());

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentViewModel>()
                .ForMember(x => x.GroupName, opt => opt.MapFrom(s => s.Group.Name)));
            var mapper = new Mapper(config);
            var studentsOnPage = mapper.Map<List<StudentViewModel>>(_unitOfWork.Students
                .GetPart(s => s.GroupId == groupId, s => s.LastName, page, StudentsPerPage));

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

            var parentNode = new MvcBreadcrumbNode("Index", "Groups", $"{currentGroup.Course.Name}") { RouteValues = new { courseId = currentGroup.CourseId } };
            var childNode1 = new MvcBreadcrumbNode("Index", "Students", $"{currentGroup.Name} group") { RouteValues = new { groupId = currentGroup.Id }, Parent = parentNode };
            var childNode2 = new MvcBreadcrumbNode("Create", "Students", "New student") { Parent = childNode1 };
            ViewData["BreadcrumbNode"] = childNode2;

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
                var config = new MapperConfiguration(cfg => cfg.CreateMap<StudentViewModel, Student>());
                var mapper = new Mapper(config);
                var student = mapper.Map<StudentViewModel, Student>(studentModel);
                _unitOfWork.Students.Add(student);
                _unitOfWork.Complete();
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData["message"] = $"Student {studentModel.FirstName} {studentModel.LastName} added";
            return RedirectToAction("Index", new { groupId = studentModel.GroupId });
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var student = _unitOfWork.Students.Get((int)id);
            if (student == null) return NotFound();

            var parentNode = new MvcBreadcrumbNode("Index", "Groups", $"{student.Group.Course.Name}") { RouteValues = new { courseId = student.Group.CourseId } };
            var childNode1 = new MvcBreadcrumbNode("Index", "Students", $"{student.Group.Name} group") { RouteValues = new { groupId = student.Group.Id }, Parent = parentNode };
            var childNode2 = new MvcBreadcrumbNode("Create", "Students", "Edit student") { Parent = childNode1 };
            ViewData["BreadcrumbNode"] = childNode2;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentViewModel>());
            var mapper = new Mapper(config);
            var studentModel = mapper.Map<Student, StudentViewModel>(student);

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

            TempData["message"] = $"Student {student.FirstName} {student.LastName} updated";
            return RedirectToAction("Index", new {groupId = student.GroupId});
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var student = _unitOfWork.Students.Get((int)id);
            if (student == null) return NotFound();

            var parentNode = new MvcBreadcrumbNode("Index", "Groups", $"{student.Group.Course.Name}") { RouteValues = new { courseId = student.Group.CourseId } };
            var childNode1 = new MvcBreadcrumbNode("Index", "Students", $"{student.Group.Name} group") { RouteValues = new { groupId = student.Group.Id }, Parent = parentNode };
            var childNode2 = new MvcBreadcrumbNode("Create", "Students", "Delete student") { Parent = childNode1 };
            ViewData["BreadcrumbNode"] = childNode2;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentViewModel>());
            var mapper = new Mapper(config);
            var studentModel = mapper.Map<Student, StudentViewModel>(student);

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

            TempData["message"] = $"Student {student.FirstName} {student.LastName} deleted";
            return RedirectToAction("Index", new {groupId = student.GroupId});
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyStudent(int id, string firstName, string lastName, DateTime dateOfBirth)
        {
            return !new DuplicateVerifier().VerifyStudent(id, firstName, lastName, dateOfBirth)
                ? Json("Such a student already exists.")
                : Json(true);
        }
    }
}
