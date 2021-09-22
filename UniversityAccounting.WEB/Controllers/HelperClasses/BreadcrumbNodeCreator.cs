using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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

        public void CreateNodes(ViewDataDictionary viewData, string action, string controller,
            string courseName = "", int courseId = 0, string groupName = "", int groupId = 0)
        {
            _parentNode = new MvcBreadcrumbNode(nameof(CoursesController.Index), "Courses", _localizer["Courses"]);
            if (controller == nameof(CoursesController).Replace("Controller", ""))
            {
                SetViewData(action, controller, viewData);
                return;
            }

            if (courseName == null || courseId < 1) throw new ArgumentException();

            _parentNode = new MvcBreadcrumbNode(nameof(GroupsController.Index), "Groups", courseName)
                { RouteValues = new { courseId }, Parent = _parentNode };
            if (controller == nameof(GroupsController).Replace("Controller", ""))
            {
                SetViewData(action, controller, viewData);
                return;
            }

            if (groupName == null || groupId < 1) throw new ArgumentException();

            _parentNode = new MvcBreadcrumbNode(nameof(StudentsController.Index), "Students", _localizer["GroupName", groupName])
                { RouteValues = new { groupId }, Parent = _parentNode };
            if (controller == nameof(StudentsController).Replace("Controller", ""))
            {
                SetViewData(action, controller, viewData);
                return;
            }

            throw new ArgumentException(null, nameof(controller));
        }

        private void SetViewData(string action, string controller, ViewDataDictionary viewData)
        {
            controller = controller.Remove(controller.Length - 1);
            viewData["BreadcrumbNode"] = action switch
            {
                "Create" => new MvcBreadcrumbNode(action, controller, _localizer["New" + controller])
                    {Parent = _parentNode},
                "Edit" => new MvcBreadcrumbNode(action, controller, _localizer["Edit" + controller])
                    {Parent = _parentNode},
                "Delete" => new MvcBreadcrumbNode(action, controller, _localizer["Delete" + controller])
                    {Parent = _parentNode},
                _ => _parentNode
            };
        }
    }
}
