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
using UniversityAccounting.DAL.BusinessLogic;

namespace UniversityAccounting.WEB.Controllers
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

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Group, GroupViewModel>()
                .ForMember(x => x.StudentsQuantity, opt => opt.MapFrom(g => g.Students.Count)));
            var mapper = new Mapper(config);
            var groupsOnPage = mapper.Map<IEnumerable<GroupViewModel>>(_unitOfWork.Groups
                .GetPart(g => g.CourseId == courseId, g => g.Name, page, GroupsPerPage));

            return View(new GroupsIndexViewModel
            {
                Groups = groupsOnPage,
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

            var parentNode = new MvcBreadcrumbNode("Index", "Groups", $"{currentCourse.Name}") {RouteValues = new {courseId = currentCourse.Id}};
            var childNode = new MvcBreadcrumbNode("Create", "Groups", "New group") {Parent = parentNode};
            ViewData["BreadcrumbNode"] = childNode;

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
                var config = new MapperConfiguration(cfg => cfg.CreateMap<GroupViewModel, Group>());
                var mapper = new Mapper(config);
                var group = mapper.Map<GroupViewModel, Group>(groupModel);
                _unitOfWork.Groups.Add(group);
                _unitOfWork.Complete();
            }
            catch (Exception)
            {
                return View("Error");
            }

            TempData["message"] = $"\"{groupModel.Name}\" group added";
            return RedirectToAction("Index", new {courseId = groupModel.CourseId});
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var group = _unitOfWork.Groups.Get((int) id);
            if (group == null) return NotFound();

            var parentNode = new MvcBreadcrumbNode("Index", "Groups", $"{group.Course.Name}") {RouteValues = new {courseId = group.Course.Id}};
            var childNode = new MvcBreadcrumbNode("Edit", "Groups", "Edit group") {Parent = parentNode};
            ViewData["BreadcrumbNode"] = childNode;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Group, GroupViewModel>());
            var mapper = new Mapper(config);
            var groupModel = mapper.Map<Group, GroupViewModel>(group);

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

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Group, GroupViewModel>());
            var mapper = new Mapper(config);
            var groupModel = mapper.Map<Group, GroupViewModel>(group);

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
                ModelState.AddModelError(string.Empty, @"Unable to delete group that contains students");
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Group, GroupViewModel>());
                var mapper = new Mapper(config);
                var groupModel = mapper.Map<Group, GroupViewModel>(group);
                return View("Delete", groupModel);
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
            return !new DuplicateVerifier().VerifyGroupName(id, name)
                ? Json($"The group name {name} is already exists.")
                : Json(true);
        }
    }
}
