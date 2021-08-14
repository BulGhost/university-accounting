using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentAccounting.Extensions
{
    public static class HtmlExtensions
    {
        private static readonly HtmlContentBuilder EmptyBuilder = new HtmlContentBuilder();

        public static IHtmlContent BuildBreadcrumbNavigation(this IHtmlHelper helper)
        {
            if (helper.ViewContext.RouteData.Values["controller"]?.ToString() == "Home" ||
                helper.ViewContext.RouteData.Values["controller"]?.ToString() == "Account")
            {
                return EmptyBuilder;
            }

            string controllerName = helper.ViewContext.RouteData.Values["controller"]?.ToString();
            string actionName = helper.ViewContext.RouteData.Values["action"]?.ToString();

            var breadcrumb = new HtmlContentBuilder()
                .AppendHtml("<ol class='breadcrumb'><li>")
                .AppendHtml(helper.ActionLink("Courses", "Index", "Courses"))
                .AppendHtml("</li><li>")
                .AppendHtml(helper.ActionLink(controllerName.Titleize(),
                    "Index", controllerName))
                .AppendHtml("</li>");


            if (helper.ViewContext.RouteData.Values["action"]?.ToString() != "Index")
            {
                breadcrumb.AppendHtml("<li>")
                    .AppendHtml(helper.ActionLink(actionName.Titleize(), actionName, controllerName))
                    .AppendHtml("</li>");
            }

            return breadcrumb.AppendHtml("</ol>");
        }
    }
}