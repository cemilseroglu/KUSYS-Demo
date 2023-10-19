using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using KUSYS_Demo.Areas.Helper;
using KUSYS_Demo.Data;
using KUSYS_Demo.Entities.Concrete;
using KUSYS_Demo.Models;
using System.Diagnostics;
using System.Text;

namespace KUSYS_Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration Configuration;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        SignInManager<AppUser> SignInManager;
        public HomeController(ILogger<HomeController> logger, IConfiguration _configuration, ApplicationDbContext context,
             UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _logger = logger;
            Configuration = _configuration;
            _userManager = userManager;
            SignInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (SignInManager.IsSignedIn(User))
            {
                return Redirect(Url.Content("~/Identity/Account/Manage"));
            }
            else
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}