using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KUSYS_Demo.Areas.Helper;
using KUSYS_Demo.Data;
using KUSYS_Demo.Entities.Concrete;
using KUSYS_Demo.Models;
using KUSYS_Demo.Data.Services.Abstract;

namespace KUSYS_Demo.Controllers
{
	public class UserController : Controller
	{
		private UserManager<AppUser> _userManager { get; }
		private RoleManager<AppRole> _roleManager { get; }
		private Toastr _toastr;
		private readonly ApplicationDbContext? _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ICourseUserService _courseUserService;
		private readonly ICourseService _courseService;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ApplicationDbContext? context, SignInManager<AppUser> signInManager, ICourseUserService courseUserService,ICourseService courseService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _signInManager = signInManager;
			_courseUserService = courseUserService;
			_courseService = courseService;
        }


		public async Task<IActionResult> Index()
		{
			return View();
		}

		public async Task<IActionResult> OwnedCourses()
		{
			var user = await _userManager.GetUserAsync(User);
			var courseUserList = _context.CourseUser.Where(x=>x.StudentId == user.Id).ToList();
			
			return View(courseUserList);
		}

		public async Task<IActionResult> CourseUserCreate(string id)
		{
			var ownablecourseList = await _courseService.GetAll();
			
			AddCourseToUserViewModel model = new AddCourseToUserViewModel();
			model.OwnableCourseList = ownablecourseList;
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> CourseUserCreate(AddCourseToUserViewModel model)
		{
			var course = await _courseService.GetById(model.CourseId);
			if(course == null)
			{
				return NotFound();
			}
			var user = await _userManager.GetUserAsync(User);

			CourseUser courseUser = new CourseUser() 
			{ 
				CourseId = model.CourseId,
				StudentId = user.StudentId,
			};

			if(_context.CourseUser.Any(x=>x.CourseId == courseUser.CourseId && x.StudentId == courseUser.StudentId))
			{
				return RedirectToAction("OwnedCourses");
			}

			await _courseUserService.Add(courseUser);
			return RedirectToAction("OwnedCourses");
		}


		public async Task<IActionResult> ChangeProfileSettings()
		{
			var user = await Task.Run(() => GetCurrentUser());
			UpdateUserViewModel updateUserViewModel = new();
			if (user != null)
			{
				updateUserViewModel.UserId = user.Id;
				updateUserViewModel.FirstName = user.FirstName;
				updateUserViewModel.LastName = user.LastName;
				updateUserViewModel.PhoneNumber = user.PhoneNumber;
				updateUserViewModel.Email = user.Email;
			}

			return View(updateUserViewModel);
		}


        [HttpPost]
        public async Task<IActionResult> ChangeProfileSettings(UpdateUserViewModel updateUserViewModel)
        {
            AppUser user = await _userManager.FindByIdAsync(updateUserViewModel.UserId);

            if (user != null && ModelState.IsValid)
            {
                user.FirstName = updateUserViewModel.FirstName;
                user.LastName = updateUserViewModel.LastName;
                user.Email = updateUserViewModel.Email;
                user.PhoneNumber = updateUserViewModel.PhoneNumber;

                await _userManager.UpdateSecurityStampAsync(user);
                await _signInManager.SignInAsync(user,isPersistent:false);
                return View(updateUserViewModel);
            }
            else
            {
                return View();
            }
        }
        private async Task<AppUser> GetCurrentUser()
		{
			var currentUserMail = await Task.Run(() => _userManager.GetUserAsync(User));
			if (currentUserMail == null || String.IsNullOrEmpty(currentUserMail.ToString()))
				return null;

			var currentUser = await Task.Run(() => _context.Users.SingleOrDefault(x => String.Equals(x.Email, currentUserMail.ToString())));
			return currentUser;

		}
	}
}
