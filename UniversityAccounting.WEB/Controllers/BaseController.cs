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
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly INotyfService Notyf;
        protected readonly IStringLocalizer<SharedResource> SharedLocalizer;
        protected readonly IMapper Mapper;
        protected readonly IBreadcrumbNodeCreator BrCrNodesCreator;

        public BaseController()
        {
        }

        public BaseController(IUnitOfWork unitOfWork, INotyfService notyf,
            IStringLocalizer<SharedResource> localizer, IMapper mapper, IBreadcrumbNodeCreator brCrNodesCreator)
        {
            UnitOfWork = unitOfWork;
            Notyf = notyf;
            SharedLocalizer = localizer;
            Mapper = mapper;
            BrCrNodesCreator = brCrNodesCreator;
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
        //TODO: Breadcrumbs optimization
    }
}