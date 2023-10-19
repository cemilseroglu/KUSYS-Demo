using Microsoft.AspNetCore.Mvc;

namespace KUSYS_Demo.Controllers
{
    public class ErrorPageController : Controller
    {
        public IActionResult Index(int code)
        {
            return View();
        }
    }
}
