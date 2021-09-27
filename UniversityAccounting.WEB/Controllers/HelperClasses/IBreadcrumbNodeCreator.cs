using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace UniversityAccounting.WEB.Controllers.HelperClasses
{
    public interface IBreadcrumbNodeCreator
    {
        void CreateNodes(ViewDataDictionary viewData, string action, string controller,
            string courseName = "", int courseId = 0, string groupName = "", int groupId = 0);
    }
}
