using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.Extensions.Localization;
using UniversityAccounting.DAL.Interfaces;
using UniversityAccounting.WEB.Controllers.HelperClasses;

namespace UniversityAccounting.WEB.Controllers
{
    public class BaseController : Controller
    {
        protected const string NotifMessage = "message";
        protected const string NotifError = "error";

        protected readonly IUnitOfWork UnitOfWork;
        protected readonly INotyfService Notyf;
        protected readonly IStringLocalizer<SharedResource> SharedLocalizer;
        protected readonly IMapper Mapper;
        protected readonly IBreadcrumbNodeCreator BreadcrumbNodeCreator;
        protected readonly ISortModel SortModel;

        public BaseController(IUnitOfWork unitOfWork, INotyfService notyf, ISortModel sortModel,
            IStringLocalizer<SharedResource> localizer, IMapper mapper, IBreadcrumbNodeCreator breadcrumbNodeCreator)
        {
            UnitOfWork = unitOfWork;
            Notyf = notyf;
            SortModel = sortModel;
            SharedLocalizer = localizer;
            Mapper = mapper;
            BreadcrumbNodeCreator = breadcrumbNodeCreator;
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions {Expires = DateTimeOffset.UtcNow.AddYears(1)}
            );

            return LocalRedirect(returnUrl);
        }
    }
}