using System.Linq;
using Microsoft.AspNetCore.Mvc;
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

        public CoursesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [DefaultBreadcrumb("Courses")]
        public IActionResult Index(int page = 1)
        {
            if (page < 1) page = 1;
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
                    TotalItems = _unitOfWork.Courses.GetAll().Count()
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
            if (ModelState.IsValid)
            {
                _unitOfWork.Courses.Add(course);
                _unitOfWork.Complete();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        [Breadcrumb("Edit course")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var course = _unitOfWork.Courses.Get((int)id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                var updCourse = _unitOfWork.Courses.Get(course.Id);
                updCourse.Name = course.Name;
                updCourse.Description = course.Description;
                _unitOfWork.Complete();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        [Breadcrumb("Delete course")]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var course = _unitOfWork.Courses.Get((int) id);
            if (course == null) return NotFound();

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var course = _unitOfWork.Courses.Get((int) id);
            if (course == null) return NotFound();

            _unitOfWork.Courses.Remove(course);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
