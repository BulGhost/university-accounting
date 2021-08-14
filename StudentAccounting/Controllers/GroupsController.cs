using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
            if (page < 1) page = 1;
            ViewBag.Course = _unitOfWork.Courses.Get(courseId);
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
