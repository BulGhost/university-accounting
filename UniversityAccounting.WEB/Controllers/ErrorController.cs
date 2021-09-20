using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace UniversityAccounting.WEB.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;
        private readonly IStringLocalizer<ErrorController> _localizer;

        public ErrorController(ILogger<ErrorController> logger, IStringLocalizer<ErrorController> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        [Route("Error/{statusCode:int}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = _localizer["404ErrorMessage"];
                    _logger.LogWarning($"404 Error Occured. Path = {statusCodeResult.OriginalPath}" +
                                       $"and QueryString = {statusCodeResult.OriginalQueryString}");
                    break;
            }

            return View("NotFound");
        }

        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            _logger.LogError($"The path {exceptionDetails.Path} threw an exception" +
                             $"{exceptionDetails.Error}");

            return View("Error");
        }
    }
}
