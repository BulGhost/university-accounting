﻿using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityAccounting.DAL.Entities;
using UniversityAccounting.DAL.Interfaces;
using UniversityAccounting.WEB.Models;
using AutoMapper;
using Microsoft.Extensions.Localization;
using UniversityAccounting.DAL.BusinessLogic;
using UniversityAccounting.WEB.Controllers.HelperClasses;
using UniversityAccounting.WEB.Models.HelperClasses;

namespace UniversityAccounting.WEB.Controllers
{
    public class GroupsController : BaseController
    {
        private const int GroupsPerPage = 10;
        private readonly IStringLocalizer<GroupsController> _localizer;

        public GroupsController(IUnitOfWork unitOfWork, INotyfService notyf, IMapper mapper,
            IBreadcrumbNodeCreator nodesCreator, ISortModel sortModel,
            IStringLocalizer<SharedResource> sharedLocalizer, IStringLocalizer<GroupsController> localizer)
            : base(unitOfWork, notyf, sortModel, sharedLocalizer, mapper, nodesCreator)
        {
            _localizer = localizer;
        }

        public IActionResult Index(int courseId, int page = 1, string sortProperty = nameof(GroupViewModel.Name),
            SortOrder sortOrder = SortOrder.Ascending, string searchText = "")
        {
            var currentCourse = UnitOfWork.Courses.Get(courseId);
            if (currentCourse == null) return NotFound();

            ViewBag.Course = currentCourse;
            int totalGroups = UnitOfWork.Groups.SuitableGroupsCount(g => g.CourseId == courseId, searchText);
            if (page < 1 || page > Math.Floor((double) totalGroups / GroupsPerPage) + 1)
                return RedirectToAction("Index", new {courseId});

            BreadcrumbNodeCreator.CreateNodes(ViewData, nameof(Index), "Groups",
                currentCourse.Name, currentCourse.Id);

            if (TempData.ContainsKey(NotifMessage)) Notyf.Success(TempData[NotifMessage].ToString());
            if (TempData.ContainsKey(NotifError)) Notyf.Error(TempData[NotifError].ToString());

            SortModel.AddColumn(nameof(GroupViewModel.Name), true);
            SortModel.AddColumn(nameof(GroupViewModel.FormationDate));
            SortModel.AddColumn(nameof(GroupViewModel.StudentsQuantity));
            SortModel.ApplySort(sortProperty, sortOrder);

            var groupsOnPage = Mapper.Map<IEnumerable<GroupViewModel>>(UnitOfWork.Groups
                .GetRequiredGroups(g => g.CourseId == courseId, searchText, sortProperty,
                    page, GroupsPerPage, sortOrder));
            ViewBag.SearchText = searchText;

            return View(new GroupsIndexViewModel
            {
                Groups = groupsOnPage,
                SortModel = SortModel,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = GroupsPerPage,
                    TotalItems = totalGroups
                }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSeveral(int?[] ids)
        {
            ICollection<Group> groups = new List<Group>();
            foreach (int? id in ids)
            {
                if (id == null) continue;

                var group = UnitOfWork.Groups.Get((int) id);
                if (group == null) return View("Error");

                groups.Add(group);
            }

            try
            {
                UnitOfWork.Groups.RemoveRange(groups);
                UnitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                TempData[NotifError] = _localizer["DeleteErrorMessage"].Value;
                return RedirectToAction("Index", new {courseId = groups.First().CourseId});
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData[NotifMessage] = _localizer["SeveralGroupsDeleted", groups.Count].Value;
            return RedirectToAction("Index", new {courseId = groups.First().CourseId});
        }

        public IActionResult Create(int? courseId)
        {
            if (courseId == null || courseId == 0) return NotFound();

            var currentCourse = UnitOfWork.Courses.Get((int) courseId);
            if (currentCourse == null) return NotFound();

            BreadcrumbNodeCreator.CreateNodes(ViewData, nameof(Create), "Groups",
                currentCourse.Name, currentCourse.Id);

            var group = new GroupViewModel {CourseId = (int) courseId, FormationDate = DateTime.Now};
            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(GroupViewModel groupModel)
        {
            if (!ModelState.IsValid) return View(groupModel);

            try
            {
                var group = Mapper.Map<GroupViewModel, Group>(groupModel);
                UnitOfWork.Groups.Add(group);
                UnitOfWork.Complete();
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData[NotifMessage] = _localizer["GroupAdded", groupModel.Name].Value;
            return RedirectToAction("Index", new {courseId = groupModel.CourseId});
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var group = UnitOfWork.Groups.Get((int) id);
            if (group == null) return NotFound();

            BreadcrumbNodeCreator.CreateNodes(ViewData, nameof(Edit), "Groups",
                group.Course.Name, group.Course.Id);

            var groupModel = Mapper.Map<Group, GroupViewModel>(group);

            return View(groupModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(GroupViewModel group)
        {
            if (!ModelState.IsValid) return View(group);

            try
            {
                var updGroup = UnitOfWork.Groups.Get(group.Id);
                updGroup.Name = group.Name;
                updGroup.CourseId = group.CourseId;
                updGroup.FormationDate = group.FormationDate;
                UnitOfWork.Complete();
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData[NotifMessage] = _localizer["GroupUpdated", group.Name].Value;
            return RedirectToAction("Index", new {courseId = group.CourseId});
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var group = UnitOfWork.Groups.Get((int) id);
            if (group == null) return NotFound();

            BreadcrumbNodeCreator.CreateNodes(ViewData, nameof(Delete), "Groups",
                group.Course.Name, group.Course.Id);

            var groupModel = Mapper.Map<Group, GroupViewModel>(group);

            return View(groupModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var group = UnitOfWork.Groups.Get((int) id);
            if (group == null) return NotFound();

            try
            {
                UnitOfWork.Groups.Remove(group);
                UnitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, _localizer["DeleteErrorMessage"]);
                var groupModel = Mapper.Map<Group, GroupViewModel>(group);
                return View("Delete", groupModel);
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData[NotifMessage] = _localizer["GroupDeleted", group.Name].Value;
            return RedirectToAction("Index", new {courseId = group.CourseId});
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyGroupName(int id, string name)
        {
            return new DuplicateVerifier(UnitOfWork).VerifyGroupName(id, name)
                ? Json(true)
                : Json(_localizer["GroupExistsErrorMessage", name].Value);
        }
    }
}