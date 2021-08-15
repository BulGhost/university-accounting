using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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

        public GroupsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int courseId, int page = 1)
        {
            var currentCourse = _unitOfWork.Courses.Get(courseId);
            var node = new MvcBreadcrumbNode("Index", "Groups", $"{currentCourse.Name}");
            ViewData["BreadcrumbNode"] = node;

            if (page < 1) page = 1;
            ViewBag.Course = currentCourse;
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

        public IActionResult Create(int courseId)
        {
            var currentCourse = _unitOfWork.Courses.Get(courseId);
            var parentNode = new MvcBreadcrumbNode("Index", "Groups", $"{currentCourse.Name}") {RouteValues = new {courseId = currentCourse.Id}};
            var childNode = new MvcBreadcrumbNode("Create", "Groups", "New group") {Parent = parentNode};
            ViewData["BreadcrumbNode"] = childNode;

            var group = new Group {CourseId = courseId, FormationDate = DateTime.Now};
            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Group group)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Groups.Add(group);
                _unitOfWork.Complete();
                return RedirectToAction("Index", new {courseId = group.CourseId});
            }

            return View(group);
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
            if (ModelState.IsValid)
            {
                var updGroup = _unitOfWork.Groups.Get(group.Id);
                updGroup.Name = group.Name;
                updGroup.CourseId = group.CourseId;
                updGroup.FormationDate = group.FormationDate;
                _unitOfWork.Complete();
                return RedirectToAction("Index", new {courseId = group.CourseId});
            }

            return View(group);
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

            _unitOfWork.Groups.Remove(group);
            _unitOfWork.Complete();
            return RedirectToAction("Index", new { courseId = group.CourseId });
        }
    }
}
