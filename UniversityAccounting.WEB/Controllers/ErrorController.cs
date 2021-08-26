using Microsoft.AspNetCore.Mvc;

namespace UniversityAccounting.WEB.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode:int}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found";
            return View("NotFound");
        }

        [Route("Error")]
        public IActionResult Error()
        {
            return View("Error");
        }
    }
}
