using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AspNetCoreHero.ToastNotification.Abstractions;
using SmartBreadcrumbs.Nodes;
using StudentAccounting.Data;
using StudentAccounting.Models;
using StudentAccounting.Models.ViewModels;

namespace StudentAccounting.Controllers
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

            return View(new StudentsIndexViewModel
            {
                Students = _unitOfWork.Students.GetAll()
                    .Where(s => s.GroupId == groupId)
                    .OrderBy(s => s.Id)
                    .Skip((page - 1) * StudentsPerPage)
                    .Take(StudentsPerPage),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = StudentsPerPage,
                    TotalItems = _unitOfWork.Students.GetAll()
                        .Count(s => s.GroupId == groupId)
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

            var student = new Student {GroupId = (int) groupId};
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            if (!ModelState.IsValid) return View(student);

            try
            {
                _unitOfWork.Students.Add(student);
                _unitOfWork.Complete();
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData["message"] = $"Student {student.FirstName} {student.LastName} added";
            return RedirectToAction("Index", new { groupId = student.GroupId });
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

            ViewBag.Groups = _unitOfWork.Groups.Find(g => g.CourseId == student.Group.CourseId);
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student student)
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

            return View(student);
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
            if (id == 0)
                return _unitOfWork.Students.Find(s =>
                    s.FirstName == firstName && s.LastName == lastName && s.DateOfBirth == dateOfBirth).Any()
                    ? Json("Such a student already exists.")
                    : Json(true);

            var studentsWithSameAttributes = _unitOfWork.Students.Find(s =>
                s.FirstName == firstName && s.LastName == lastName && s.DateOfBirth == dateOfBirth);
            return studentsWithSameAttributes.Any(student => student.Id != id)
                ? Json("Such a student already exists.")
                : Json(true);
        }
    }
}
