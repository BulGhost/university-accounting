using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBreadcrumbs.Nodes;
using UniversityAccounting.DAL.Entities;
using UniversityAccounting.DAL.Interfaces;
using UniversityAccounting.WEB.Models;
using AutoMapper;
using Microsoft.Extensions.Localization;
using UniversityAccounting.DAL.BusinessLogic;
using UniversityAccounting.WEB.Models.HelperClasses;

namespace UniversityAccounting.WEB.Controllers
{
    public class GroupsController : Controller
    {
        private const int GroupsPerPage = 10;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService _notyf;
        private readonly IStringLocalizer<GroupsController> _localizer;
        private readonly IMapper _mapper;

        public GroupsController(IUnitOfWork unitOfWork, INotyfService notyf,
            IStringLocalizer<GroupsController> localizer, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _notyf = notyf;
            _localizer = localizer;
            _mapper = mapper;
        }

        public IActionResult Index(int courseId, int page = 1, string sortProperty = nameof(Group.Name),
            SortOrder sortOrder = SortOrder.Ascending)
        {
            var currentCourse = _unitOfWork.Courses.Get(courseId);
            if (currentCourse == null) return NotFound();

            ViewBag.Course = currentCourse;
            int totalGroups = _unitOfWork.Groups.Find(g => g.CourseId == courseId).Count();
            if (page < 1 || page > Math.Ceiling((double) totalGroups / GroupsPerPage))
                return RedirectToAction("Index", new {page = 1});

            var node1 = new MvcBreadcrumbNode("Index", "Courses", _localizer["Courses"]);
            var node2 = new MvcBreadcrumbNode("Index", "Groups", $"{currentCourse.Name}") {Parent = node1};
            ViewData["BreadcrumbNode"] = node2;

            if (TempData.ContainsKey("message")) _notyf.Success(TempData["message"].ToString());

            var sortModel = new SortModel();
            sortModel.AddColumn(nameof(Group.Name), true);
            sortModel.AddColumn(nameof(Group.FormationDate));
            sortModel.AddColumn(nameof(Group.StudentsQuantity));
            sortModel.ApplySort(sortProperty, sortOrder);

            var groupsOnPage = _mapper.Map<IEnumerable<GroupViewModel>>(_unitOfWork.Groups
                .GetPart(g => g.CourseId == courseId, sortProperty, page, GroupsPerPage, sortOrder));

            return View(new GroupsIndexViewModel
            {
                Groups = groupsOnPage,
                SortModel = sortModel,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = GroupsPerPage,
                    TotalItems = _unitOfWork.Groups.Find(g => g.CourseId == courseId).Count()
                }
            });
        }

        public IActionResult Create(int? courseId)
        {
            if (courseId == null || courseId == 0) return NotFound();

            var currentCourse = _unitOfWork.Courses.Get((int) courseId);
            if (currentCourse == null) return NotFound();

            var node1 = new MvcBreadcrumbNode("Index", "Courses", _localizer["Courses"]);
            var node2 = new MvcBreadcrumbNode("Index", "Groups", $"{currentCourse.Name}")
                {RouteValues = new {courseId = currentCourse.Id}, Parent = node1};
            var node3 = new MvcBreadcrumbNode("Create", "Groups", _localizer["NewGroup"]) {Parent = node2};
            ViewData["BreadcrumbNode"] = node3;

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
                var group = _mapper.Map<GroupViewModel, Group>(groupModel);
                _unitOfWork.Groups.Add(group);
                _unitOfWork.Complete();
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData["message"] = _localizer["GroupAdded", groupModel.Name].Value;
            return RedirectToAction("Index", new {courseId = groupModel.CourseId});
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var group = _unitOfWork.Groups.Get((int) id);
            if (group == null) return NotFound();

            var node1 = new MvcBreadcrumbNode("Index", "Courses", _localizer["Courses"]);
            var node2 = new MvcBreadcrumbNode("Index", "Groups", $"{group.Course.Name}")
                {RouteValues = new {courseId = group.Course.Id}, Parent = node1};
            var node3 = new MvcBreadcrumbNode("Edit", "Groups", _localizer["EditGroup"]) {Parent = node2};
            ViewData["BreadcrumbNode"] = node3;

            var groupModel = _mapper.Map<Group, GroupViewModel>(group);

            return View(groupModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(GroupViewModel group)
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

            TempData["message"] = _localizer["GroupUpdated", group.Name].Value;
            return RedirectToAction("Index", new {courseId = group.CourseId});
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var group = _unitOfWork.Groups.Get((int) id);
            if (group == null) return NotFound();

            var node1 = new MvcBreadcrumbNode("Index", "Courses", _localizer["Courses"]);
            var node2 = new MvcBreadcrumbNode("Index", "Groups", $"{group.Course.Name}")
                {RouteValues = new {courseId = group.Course.Id}, Parent = node1};
            var node3 = new MvcBreadcrumbNode("Edit", "Groups", _localizer["DeleteGroup"]) {Parent = node2};
            ViewData["BreadcrumbNode"] = node3;

            var groupModel = _mapper.Map<Group, GroupViewModel>(group);

            return View(groupModel);
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
                ModelState.AddModelError(string.Empty, _localizer["DeleteErrorMessage"]);
                var groupModel = _mapper.Map<Group, GroupViewModel>(group);
                return View("Delete", groupModel);
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData["message"] = _localizer["GroupDeleted", group.Name].Value;
            return RedirectToAction("Index", new {courseId = group.CourseId});
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyGroupName(int id, string name)
        {
            return new DuplicateVerifier().VerifyGroupName(id, name)
                ? Json(true)
                : Json(_localizer["GroupExistsErrorMessage", name].Value);
        }
    }
}