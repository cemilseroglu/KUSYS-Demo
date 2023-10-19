using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using KUSYS_Demo.Data;
using KUSYS_Demo.Entities.Concrete;
using KUSYS_Demo.Models;
using KUSYS_Demo.Areas.Helper;
using KUSYS_Demo.Data.Services.Abstract;
using KUSYS_Demo.Data.Utilities.Config;

namespace KUSYS_Demo.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private UserManager<AppUser> _userManager { get; }
		private RoleManager<AppRole> _roleManager { get; }
		private readonly ApplicationDbContext? _context;
		private readonly ICourseService _courseService;

		public AdminController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ApplicationDbContext context, IToastNotification toastNotification, ICourseService courseService)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
			_courseService = courseService;
		}

		public IActionResult Index()
		{

			return View(User.Claims.ToList());
		}

		public IActionResult Users()
		{
			return View(_userManager.Users.ToList());
		}

		public async Task<IActionResult> UpdateUser(string id)
		{
			AppUser user = await _userManager.FindByIdAsync(id);

			if (user != null)
			{
				UpdateUserViewModel updateUserViewModel = new();
				updateUserViewModel.UserId = user.Id;
				updateUserViewModel.FirstName = user.FirstName;
				updateUserViewModel.LastName = user.LastName;
				updateUserViewModel.PhoneNumber = user.PhoneNumber;
				updateUserViewModel.Email = user.Email;

				return View(updateUserViewModel);
			}
			else
			{
				return RedirectToAction("Users");
			}
		}

		[HttpPost]
		public async Task<IActionResult> UpdateUser(UpdateUserViewModel updateUserViewModel)
		{
			AppUser user = await _userManager.FindByIdAsync(updateUserViewModel.UserId);

			if (user != null && ModelState.IsValid)
			{
				user.FirstName = updateUserViewModel.FirstName;
				user.LastName = updateUserViewModel.LastName;
				user.Email = updateUserViewModel.Email;
				user.PhoneNumber = updateUserViewModel.PhoneNumber;

				await _userManager.UpdateSecurityStampAsync(user);
				return RedirectToAction("Users");
			}
			else
			{
				return View();
			}
		}

		public IActionResult UserDelete(string id)
		{
			var userSelector = _userManager.FindByIdAsync(id).Result;
			if (userSelector != null)
			{
				IdentityResult result = _userManager.DeleteAsync(userSelector).Result;
				if (result.Succeeded)
				{
					return RedirectToAction("Users", userSelector);
				}
				else
				{
					return RedirectToAction("Users");
				}

			}
			else
			{
				return RedirectToAction("Users");
			}
		}
		public IActionResult RoleCreate()
		{
			return View();
		}
		[HttpPost]
		public IActionResult RoleCreate(RoleViewModel roleViewModel)
		{
			AppRole role = new AppRole();
			role.Name = roleViewModel.Name;
			IdentityResult result = _roleManager.CreateAsync(role).Result;
			if (result.Succeeded)
			{
				return RedirectToAction("Roles");
			}
			else
			{
				foreach (var item in result.Errors)
				{
					ModelState.AddModelError("", item.Description);
				}
			}
			return View(roleViewModel);
		}

		public IActionResult Roles()
		{
			return View(_roleManager.Roles.ToList());
		}

		public IActionResult RoleDelete(string id)
		{
			var roleSelector = _roleManager.FindByIdAsync(id).Result;
			if (roleSelector != null)
			{
				IdentityResult result = _roleManager.DeleteAsync(roleSelector).Result;
				if (result.Succeeded)
				{
					return RedirectToAction("Roles", roleSelector);
				}
				else
				{
					return RedirectToAction("Roles");
				}

			}
			else
			{
				return RedirectToAction("Roles");
			}
		}
		public IActionResult RoleUpdate(string id)
		{
			AppRole roleSelector = _roleManager.FindByIdAsync(id).Result;
			if (roleSelector != null)
			{
				return View(roleSelector.Adapt<RoleViewModel>());
			}
			return RedirectToAction("Roles");
		}

		[HttpPost]
		public IActionResult RoleUpdate(RoleViewModel roleViewModel)
		{
			var roleSelector = _roleManager.FindByIdAsync(roleViewModel.Id).Result;
			if (roleSelector != null)
			{
				roleSelector.Name = roleViewModel.Name;
				IdentityResult result = _roleManager.UpdateAsync(roleSelector).Result;
				if (result.Succeeded)
				{
					return RedirectToAction("Roles", roleSelector);
				}
				else
				{
					return RedirectToAction("Roles");
				}

			}
			else
			{
				return RedirectToAction("Roles");
			}
		}

		public IActionResult RoleAssign(string id)
		{
			TempData["userId"] = id;
			AppUser user = _userManager.FindByIdAsync(id).Result;
			if (user != null)
			{
				IQueryable<AppRole> roles = _roleManager.Roles;
				List<string>? userRoles = _userManager.GetRolesAsync(user).Result as List<string>;
				List<RoleAssignViewModel> roleAssignViewModels = new List<RoleAssignViewModel>();
				foreach (var role in roles)
				{
					RoleAssignViewModel r = new RoleAssignViewModel();
					r.RoleId = role.Id;
					r.RoleName = role.Name;
					if (userRoles.Contains(role.Name))
					{
						r.Exist = true;
					}
					else
					{
						r.Exist = false;
					}
					roleAssignViewModels.Add(r);

				}
				return View(roleAssignViewModels);
			}

			else
			{
				return RedirectToAction("Users");
			}
		}

		[HttpPost]
		public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> roleAssignViewModels)
		{
			AppUser user = _userManager.FindByIdAsync(TempData["userId"].ToString()).Result;
			foreach (var item in roleAssignViewModels)
			{
				if (item.Exist)
				{
					await _userManager.AddToRoleAsync(user, item.RoleName);
					await _userManager.UpdateSecurityStampAsync(user);
				}
				else
				{
					await _userManager.RemoveFromRoleAsync(user, item.RoleName);
					await _userManager.UpdateSecurityStampAsync(user);
				}
			}

			return RedirectToAction("Users");
		}

		private async Task<AppUser> GetCurrentUser()
		{
			var currentUserMail = await Task.Run(() => _userManager.GetUserAsync(User));
			if (currentUserMail == null || String.IsNullOrEmpty(currentUserMail.ToString()))
				return null;

			var currentUser = await Task.Run(() => _context.Users.SingleOrDefault(x => String.Equals(x.Email, currentUserMail.ToString())));
			return currentUser;

		}


		public IActionResult UserCreate()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> UserCreate(UserCreateViewModel vm)
		{
			Random random = new Random();
			AppUser newUser = new AppUser()
			{
				StudentId = random.Next(100000, 1000000).ToString(),
				FirstName = vm.FirstName,
				LastName = vm.LastName,
				Email = vm.Email,
				EmailConfirmed = true,
				UserName = vm.Email.Normalize(),
				NormalizedEmail = vm.Email.Normalize(),
				BirthDate = vm.BirthDate,
			};

			while (_userManager.Users.Any(x => x.StudentId == newUser.StudentId))
			{
				newUser.StudentId = random.Next(100000, 1000000).ToString();
			}

			if (vm.Password.Equals(vm.ConfirmPassword))
			{
				var result = await _userManager.CreateAsync(newUser, vm.Password);
				await _userManager.AddToRoleAsync(newUser, SystemConfig.StudentRole);

				return RedirectToAction("Users", "Admin");
			}
			else
			{
				return View(vm);
			}
		}
	}
}
