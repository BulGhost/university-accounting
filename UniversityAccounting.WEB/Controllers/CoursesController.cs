﻿using System;
using System.Collections.Generic;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityAccounting.DAL.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Localization;
using UniversityAccounting.DAL.BusinessLogic;
using UniversityAccounting.DAL.Entities;
using UniversityAccounting.WEB.Controllers.HelperClasses;
using UniversityAccounting.WEB.Models;
using UniversityAccounting.WEB.Models.HelperClasses;

namespace UniversityAccounting.WEB.Controllers
{
    public class CoursesController : BaseController
    {
        private const int CoursesPerPage = 5;
        private readonly IStringLocalizer<CoursesController> _localizer;

        public CoursesController(IUnitOfWork unitOfWork, INotyfService notyf, IMapper mapper,
            IBreadcrumbNodeCreator nodesCreator, ISortModel sortModel,
            IStringLocalizer<SharedResource> sharedLocalizer, IStringLocalizer<CoursesController> localizer)
            : base(unitOfWork, notyf, sortModel, sharedLocalizer, mapper, nodesCreator)
        {
            _localizer = localizer;
        }

        public IActionResult Index(int page = 1, string sortProperty = nameof(CourseViewModel.Name),
            SortOrder sortOrder = SortOrder.Ascending, string searchText = "")
        {
            int totalCourses = UnitOfWork.Courses.SuitableCoursesCount(searchText);
            if (page < 1 || page > Math.Floor((double) totalCourses / CoursesPerPage) + 1)
                return RedirectToAction("Index", new {page = 1});

            if (TempData.ContainsKey(NotifMessage)) Notyf.Success(TempData[NotifMessage].ToString());
            if (TempData.ContainsKey(NotifError)) Notyf.Error(TempData[NotifError].ToString());

            SortModel.AddColumn(nameof(CourseViewModel.Name), true);
            SortModel.AddColumn(nameof(CourseViewModel.Description));
            SortModel.ApplySort(sortProperty, sortOrder);

            var coursesOnPage = Mapper.Map<IEnumerable<CourseViewModel>>(
                UnitOfWork.Courses.GetRequiredCourses(searchText, sortProperty,
                    page, CoursesPerPage, sortOrder));
            ViewBag.SearchText = searchText;

            return View(new CoursesIndexViewModel
            {
                Courses = coursesOnPage,
                SortModel = SortModel,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = CoursesPerPage,
                    TotalItems = totalCourses
                }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSeveral(int?[] ids)
        {
            ICollection<Course> courses = new List<Course>();
            foreach (int? id in ids)
            {
                if (id == null) continue;

                var course = UnitOfWork.Courses.Get((int) id);
                if (course == null) return View("Error");

                courses.Add(course);
            }

            try
            {
                UnitOfWork.Courses.RemoveRange(courses);
                UnitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                TempData[NotifError] = _localizer["DeleteErrorMessage"].Value;
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData[NotifMessage] = _localizer["SeveralCoursesDeleted", courses.Count].Value;
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            BreadcrumbNodeCreator.CreateNodes(ViewData, nameof(Create), "Courses");

            return View(new CourseViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CourseViewModel courseModel)
        {
            if (!ModelState.IsValid) return View(courseModel);

            try
            {
                var course = Mapper.Map<CourseViewModel, Course>(courseModel);
                UnitOfWork.Courses.Add(course);
                UnitOfWork.Complete();
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData[NotifMessage] = _localizer["CourseAdded", courseModel.Name].Value;
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var course = UnitOfWork.Courses.Get((int)id);
            if (course == null) return NotFound();

            BreadcrumbNodeCreator.CreateNodes(ViewData, nameof(Edit), "Courses");

            var courseModel = Mapper.Map<Course, CourseViewModel>(course);

            return View(courseModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CourseViewModel courseModel)
        {
            if (!ModelState.IsValid) return View(courseModel);

            try
            {
                var course = UnitOfWork.Courses.Get(courseModel.Id);
                course.Name = courseModel.Name;
                course.Description = courseModel.Description;
                UnitOfWork.Complete();
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData[NotifMessage] = _localizer["CourseUpdated", courseModel.Name].Value;
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var course = UnitOfWork.Courses.Get((int)id);
            if (course == null) return NotFound();

            BreadcrumbNodeCreator.CreateNodes(ViewData, nameof(Delete), "Courses");

            var courseModel = Mapper.Map<Course, CourseViewModel>(course);

            return View(courseModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var course = UnitOfWork.Courses.Get((int)id);
            if (course == null) return NotFound();

            try
            {
                UnitOfWork.Courses.Remove(course);
                UnitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, _localizer["DeleteErrorMessage"]);
                var courseModel = Mapper.Map<Course, CourseViewModel>(course);
                return View("Delete", courseModel);
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData[NotifMessage] = _localizer["CourseDeleted", course.Name].Value;
            return RedirectToAction("Index");
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyCourseName(int id, string name)
        {
            return new DuplicateVerifier(UnitOfWork).VerifyCourseName(id, name)
                ? Json(true)
                : Json(_localizer["CourseExistsErrorMessage", name].Value);
        }
    }
}
