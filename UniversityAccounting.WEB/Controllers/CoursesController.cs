using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBreadcrumbs.Attributes;
using UniversityAccounting.DAL.Interfaces;
using AutoMapper;
using UniversityAccounting.DAL.Entities;
using UniversityAccounting.WEB.Models;

namespace UniversityAccounting.WEB.Controllers
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
            if (page < 1 || page > Math.Ceiling((double) totalCourses / CoursesPerPage))
                return RedirectToAction("Index", new {page = 1});

            if (TempData.ContainsKey("message")) _notyf.Success(TempData["message"].ToString());

            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Course, CourseViewModel>());
            var mapper = new Mapper(config);
            var allCourses = mapper.Map<IEnumerable<CourseViewModel>>(_unitOfWork.Courses.GetAll());

            return View(new CoursesIndexViewModel
            {
                Courses = allCourses
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
        public IActionResult Create(CourseViewModel courseModel)
        {
            if (!ModelState.IsValid) return View(courseModel);

            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<CourseViewModel, Course>());
                var mapper = new Mapper(config);
                var course = mapper.Map<CourseViewModel, Course>(courseModel);
                _unitOfWork.Courses.Add(course);
                _unitOfWork.Complete();
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData["message"] = $"\"{courseModel.Name}\" course added";
            return RedirectToAction("Index");
        }

        [Breadcrumb("Edit course")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var course = _unitOfWork.Courses.Get((int) id);
            if (course == null) return NotFound();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Course, CourseViewModel>());
            var mapper = new Mapper(config);
            var courseModel = mapper.Map<Course, CourseViewModel>(course);

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

            TempData["message"] = $"\"{courseModel.Name}\" course updated";
            return RedirectToAction("Index");
        }

        [Breadcrumb("Delete course")]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var course = _unitOfWork.Courses.Get((int) id);
            if (course == null) return NotFound();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Course, CourseViewModel>());
            var mapper = new Mapper(config);
            var courseModel = mapper.Map<Course, CourseViewModel>(course);

            return View(courseModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var course = _unitOfWork.Courses.Get((int) id);
            if (course == null) return NotFound();

            try
            {
                _unitOfWork.Courses.Remove(course);
                _unitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, @"Unable to delete course that contains groups");
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Course, CourseViewModel>());
                var mapper = new Mapper(config);
                var courseModel = mapper.Map<Course, CourseViewModel>(course);
                return View("Delete", courseModel);
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
