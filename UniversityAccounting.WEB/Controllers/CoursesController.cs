using System;
using System.Collections.Generic;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityAccounting.DAL.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Localization;
using SmartBreadcrumbs.Nodes;
using UniversityAccounting.DAL.BusinessLogic;
using UniversityAccounting.DAL.Entities;
using UniversityAccounting.WEB.Models;
using UniversityAccounting.WEB.Models.HelperClasses;

namespace UniversityAccounting.WEB.Controllers
{
    public class CoursesController : Controller
    {
        private const int CoursesPerPage = 5;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService _notyf;
        private readonly IStringLocalizer<CoursesController> _localizer;
        private readonly IMapper _mapper;

        public CoursesController(IUnitOfWork unitOfWork, INotyfService notyf,
            IStringLocalizer<CoursesController> localizer, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _notyf = notyf;
            _localizer = localizer;
            _mapper = mapper;
        }

        public IActionResult Index(int page = 1, string sortProperty = nameof(Course.Id),
            SortOrder sortOrder = SortOrder.Ascending)
        {
            int totalCourses = _unitOfWork.Courses.TotalCount();
            if (page < 1 || page > Math.Ceiling((double)totalCourses / CoursesPerPage))
                return RedirectToAction("Index", new {page = 1});

            if (TempData.ContainsKey("message")) _notyf.Success(TempData["message"].ToString());

            var sortModel = new SortModel();
            sortModel.AddColumn(nameof(Course.Name));
            sortModel.AddColumn(nameof(Course.Description));
            sortModel.ApplySort(sortProperty, sortOrder);

            var coursesOnPage = _mapper.Map<IEnumerable<CourseViewModel>>(
                _unitOfWork.Courses.GetPart(sortProperty, page, CoursesPerPage, sortOrder));

            return View(new CoursesIndexViewModel
            {
                Courses = coursesOnPage,
                SortModel = sortModel,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = CoursesPerPage,
                    TotalItems = totalCourses
                }
            });
        }

        public IActionResult Create()
        {
            var node1 = new MvcBreadcrumbNode("Index", "Courses", _localizer["Courses"]);
            var node2 = new MvcBreadcrumbNode("Create", "Courses", _localizer["CreateNew"]) { Parent = node1 };
            ViewData["BreadcrumbNode"] = node2;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CourseViewModel courseModel)
        {
            if (!ModelState.IsValid) return View(courseModel);

            try
            {
                var course = _mapper.Map<CourseViewModel, Course>(courseModel);
                _unitOfWork.Courses.Add(course);
                _unitOfWork.Complete();
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData["message"] = _localizer["CourseAdded", courseModel.Name].Value;
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var course = _unitOfWork.Courses.Get((int)id);
            if (course == null) return NotFound();

            var node1 = new MvcBreadcrumbNode("Index", "Courses", _localizer["Courses"]);
            var node2 = new MvcBreadcrumbNode("Edit", "Courses", _localizer["EditCourse"]) { Parent = node1 };
            ViewData["BreadcrumbNode"] = node2;

            var courseModel = _mapper.Map<Course, CourseViewModel>(course);

            return View(courseModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CourseViewModel courseModel)
        {
            if (!ModelState.IsValid) return View(courseModel);

            try
            {
                var course = _unitOfWork.Courses.Get(courseModel.Id);
                course.Name = courseModel.Name;
                course.Description = courseModel.Description;
                _unitOfWork.Complete();
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData["message"] = _localizer["CourseUpdated", courseModel.Name].Value;
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var course = _unitOfWork.Courses.Get((int)id);
            if (course == null) return NotFound();

            var node1 = new MvcBreadcrumbNode("Index", "Courses", _localizer["Courses"]);
            var node2 = new MvcBreadcrumbNode("Delete", "Courses", _localizer["DeleteCourse"]) { Parent = node1 };
            ViewData["BreadcrumbNode"] = node2;

            var courseModel = _mapper.Map<Course, CourseViewModel>(course);

            return View(courseModel);
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
                ModelState.AddModelError(string.Empty, _localizer["DeleteErrorMessage"]);
                var courseModel = _mapper.Map<Course, CourseViewModel>(course);
                return View("Delete", courseModel);
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData["message"] = _localizer["CourseDeleted", course.Name].Value;
            return RedirectToAction("Index");
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyCourseName(int id, string name)
        {
            return new DuplicateVerifier().VerifyCourseName(id, name)
                ? Json(true)
                : Json(_localizer["CourseExistsErrorMessage", name].Value);
        }
    }
}
