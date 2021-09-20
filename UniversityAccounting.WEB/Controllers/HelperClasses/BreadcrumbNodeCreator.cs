using System;
using Microsoft.Extensions.Localization;
using SmartBreadcrumbs.Nodes;

namespace UniversityAccounting.WEB.Controllers.HelperClasses
{
    public class BreadcrumbNodeCreator : IBreadcrumbNodeCreator
    {
        private MvcBreadcrumbNode _parentNode;
        private readonly IStringLocalizer<BreadcrumbNodeCreator> _localizer;

        public BreadcrumbNodeCreator(IStringLocalizer<BreadcrumbNodeCreator> localizer)
        {
            _localizer = localizer;
        }

        public MvcBreadcrumbNode CreateNodes(string action, string controller,
            string courseName = "", int courseId = 0, string groupName = "", int groupId = 0)
        {
            _parentNode = new MvcBreadcrumbNode(nameof(CoursesController.Index), "Courses", _localizer["Courses"]);
            if (controller == "Courses")
                return GetNodesForCourses(action, controller);

            if (courseName == null || courseId < 1) throw new ArgumentException();

            _parentNode = new MvcBreadcrumbNode(nameof(GroupsController.Index), "Groups", courseName)
                {RouteValues = new {courseId}, Parent = _parentNode};
            if (controller == "Groups")
                return GetNodesForGroups(action, controller);

            if (groupName == null || groupId < 1) throw new ArgumentException();

            _parentNode = new MvcBreadcrumbNode(nameof(StudentsController.Index), "Students", _localizer["GroupName", groupName])
                {RouteValues = new {groupId}, Parent = _parentNode};
            if (controller == "Students")
                return GetNodesForStudents(action, controller);

            throw new ArgumentException(null, nameof(controller));
        }

        private MvcBreadcrumbNode GetNodesForCourses(string action, string controller)
        {
            return action switch
            {
                "Create" => new MvcBreadcrumbNode(action, controller, _localizer["NewCourse"]) {Parent = _parentNode},
                "Edit" => new MvcBreadcrumbNode(action, controller, _localizer["EditCourse"]) {Parent = _parentNode},
                "Delete" => new MvcBreadcrumbNode(action, controller, _localizer["DeleteCourse"]) {Parent = _parentNode},
                _ => _parentNode
            };
        }

        private MvcBreadcrumbNode GetNodesForGroups(string action, string controller)
        {
            return action switch
            {
                "Create" => new MvcBreadcrumbNode(action, controller, _localizer["NewGroup"]) {Parent = _parentNode},
                "Edit" => new MvcBreadcrumbNode(action, controller, _localizer["EditGroup"]) {Parent = _parentNode},
                "Delete" => new MvcBreadcrumbNode(action, controller, _localizer["DeleteGroup"]) {Parent = _parentNode},
                _ => _parentNode
            };
        }

        private MvcBreadcrumbNode GetNodesForStudents(string action, string controller)
        {
            return action switch
            {
                "Create" => new MvcBreadcrumbNode(action, controller, _localizer["NewStudent"]) {Parent = _parentNode},
                "Edit" => new MvcBreadcrumbNode(action, controller, _localizer["EditStudent"]) {Parent = _parentNode},
                "Delete" => new MvcBreadcrumbNode(action, controller, _localizer["DeleteStudent"]) {Parent = _parentNode},
                _ => _parentNode
            };
        }
    }
}
