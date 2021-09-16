using SmartBreadcrumbs.Nodes;

namespace UniversityAccounting.WEB.Controllers.HelperClasses
{
    public interface IBreadcrumbNodeCreator
    {
        MvcBreadcrumbNode CreateNodes(string action, string controller,
            string courseName = "", int courseId = 0, string groupName = "", int groupId = 0);
    }
}
