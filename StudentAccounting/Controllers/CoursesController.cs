using System;
using System.Linq;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBreadcrumbs.Attributes;
using StudentAccounting.Data;
using StudentAccounting.Models;
using StudentAccounting.Models.ViewModels;

namespace StudentAccounting.Controllers
{
    public class CoursesController : Controller
    {
        private const int CoursesPerPage = 5;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService _notyf;

        public CoursesController(IUnitOfWork unitOfWork, INotyfService notyf)
        {
            _unitOfWork = unitOfWork;
            _notyf = notyf;
        }

        [DefaultBreadcrumb("Courses")]
        public IActionResult Index(int page = 1)
        {
            int totalCourses = _unitOfWork.Courses.GetAll().Count();
            if (page < 1 || page > Math.Ceiling((double)totalCourses / CoursesPerPage))
                return RedirectToAction("Index", new {page = 1});

            if (TempData.ContainsKey("message")) _notyf.Success(TempData["message"].ToString());

            return View(new CoursesIndexViewModel
            {
                Courses = _unitOfWork.Courses.GetAll()
                    .OrderBy(c => c.Id)
                    .Skip((page - 1) * CoursesPerPage)
                    .Take(CoursesPerPage),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = CoursesPerPage,
                    TotalItems = totalCourses
                }
            });
        }

        [Breadcrumb("Create new")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course)
        {
            if (!ModelState.IsValid) return View(course);

            try
            {
                _unitOfWork.Courses.Add(course);
                _unitOfWork.Complete();
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData["message"] = $"\"{course.Name}\" course added";
            return RedirectToAction("Index");
        }

        [Breadcrumb("Edit course")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var course = _unitOfWork.Courses.Get((int)id);
            if (course == null) return NotFound();

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Course course)
        {
            if (!ModelState.IsValid) return View(course);

            try
            {
                var updCourse = _unitOfWork.Courses.Get(course.Id);
                updCourse.Name = course.Name;
                updCourse.Description = course.Description;
                _unitOfWork.Complete();
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData["message"] = $"\"{course.Name}\" course updated";
            return RedirectToAction("Index");
        }

        [Breadcrumb("Delete course")]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var course = _unitOfWork.Courses.Get((int)id);
            if (course == null) return NotFound();

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var course = _unitOfWork.Courses.Get((int)id);
            if (course == null) return NotFound();

            try
            {
                _unitOfWork.Courses.Remove(course);
                _unitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, @"Unable to delete course that contains groups");
                return View("Delete", course);
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData["message"] = $"\"{course.Name}\" course deleted";
            return RedirectToAction("Index");
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyCourseName(int id, string name)
        {
            if (id == 0)
                return _unitOfWork.Courses.Find(c => c.Name == name).Any()
                    ? Json($"The course name {name} is already exists.")
                    : Json(true);

            var coursesWithSameName = _unitOfWork.Courses.Find(c => c.Name == name);
            return coursesWithSameName.Any(course => course.Id != id)
                ? Json($"The course name {name} is already exists.")
                : Json(true);
        }
    }
}
