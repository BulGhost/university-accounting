using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using SmartBreadcrumbs.Nodes;
using StudentAccounting.Data;
using StudentAccounting.Models;
using StudentAccounting.Models.ViewModels;

namespace StudentAccounting.Controllers
{
    public class GroupsController : Controller
    {
        private const int GroupsPerPage = 10;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService _notyf;

        public GroupsController(IUnitOfWork unitOfWork, INotyfService notyf)
        {
            _unitOfWork = unitOfWork;
            _notyf = notyf;
        }

        public IActionResult Index(int courseId, int page = 1)
        {
            var currentCourse = _unitOfWork.Courses.Get(courseId);
            if (currentCourse == null) return NotFound();

            var node = new MvcBreadcrumbNode("Index", "Groups", $"{currentCourse.Name}");
            ViewData["BreadcrumbNode"] = node;

            if (page < 1) page = 1;
            ViewBag.Course = currentCourse;
            if (TempData.ContainsKey("message")) _notyf.Success(TempData["message"].ToString());

            return View(new GroupsIndexViewModel
            {
                Groups = _unitOfWork.Groups.GetAll()
                    .Where(g => g.CourseId == courseId)
                    .OrderBy(g => g.Id)
                    .Skip((page - 1) * GroupsPerPage)
                    .Take(GroupsPerPage),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = GroupsPerPage,
                    TotalItems = _unitOfWork.Groups
                        .GetAll()
                        .Count(g => g.CourseId == courseId)
                }
            });
        }

        public IActionResult Create(int? courseId)
        {
            if (courseId == null || courseId == 0) return NotFound();

            var currentCourse = _unitOfWork.Courses.Get((int) courseId);
            if (currentCourse == null) return NotFound();

            var parentNode = new MvcBreadcrumbNode("Index", "Groups", $"{currentCourse.Name}") {RouteValues = new {courseId = currentCourse.Id}};
            var childNode = new MvcBreadcrumbNode("Create", "Groups", "New group") {Parent = parentNode};
            ViewData["BreadcrumbNode"] = childNode;

            var group = new Group {CourseId = (int) courseId, FormationDate = DateTime.Now};
            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Group group)
        {
            if (!ModelState.IsValid) return View(group);

            try
            {
                _unitOfWork.Groups.Add(group);
                _unitOfWork.Complete();
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData["message"] = $"\"{group.Name}\" group added";
            return RedirectToAction("Index", new {courseId = group.CourseId});
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var group = _unitOfWork.Groups.Get((int) id);
            if (group == null) return NotFound();

            var parentNode = new MvcBreadcrumbNode("Index", "Groups", $"{group.Course.Name}") {RouteValues = new {courseId = group.Course.Id}};
            var childNode = new MvcBreadcrumbNode("Edit", "Groups", "Edit group") {Parent = parentNode};
            ViewData["BreadcrumbNode"] = childNode;

            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Group group)
        {
            if (!ModelState.IsValid) return View(group);

            try
            {
                var updGroup = _unitOfWork.Groups.Get(group.Id);
                updGroup.Name = group.Name;
                updGroup.CourseId = group.CourseId;
                updGroup.FormationDate = group.FormationDate;
                _unitOfWork.Complete();
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData["message"] = $"\"{group.Name}\" group updated";
            return RedirectToAction("Index", new {courseId = group.CourseId});
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var group = _unitOfWork.Groups.Get((int) id);
            if (group == null) return NotFound();

            var parentNode = new MvcBreadcrumbNode("Index", "Groups", $"{group.Course.Name}") {RouteValues = new {courseId = group.Course.Id}};
            var childNode = new MvcBreadcrumbNode("Edit", "Groups", "Delete group") {Parent = parentNode};
            ViewData["BreadcrumbNode"] = childNode;

            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var group = _unitOfWork.Groups.Get((int) id);
            if (group == null) return NotFound();

            try
            {
                _unitOfWork.Groups.Remove(group);
                _unitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, @"Unable to delete group that contains students");
                return View("Delete", group);
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData["message"] = $"\"{group.Name}\" group deleted";
            return RedirectToAction("Index", new { courseId = group.CourseId });
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyGroupName(int id, string name)
        {
            if (id == 0)
                return _unitOfWork.Groups.Find(g => g.Name == name).Any()
                    ? Json($"The group name {name} is already exists.")
                    : Json(true);

            var groupsWithSameName = _unitOfWork.Groups.Find(g => g.Name == name);
            return groupsWithSameName.Any(group => group.Id != id)
                ? Json($"The group name {name} is already exists.")
                : Json(true);
        }
    }
}
