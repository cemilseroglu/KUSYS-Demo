// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using KUSYS_Demo.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KUSYS_Demo.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<AppRole> _userRoleManager;

		public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, 
            RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
			_userRoleManager = roleManager;
		}


		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public string Username { get; set; }
		public IList<string> UserRoleList { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[TempData]
		public string StatusMessage { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[BindProperty]
		public InputModel Input { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public class InputModel
		{

			public string FirstName { get; set; }
			public string LastName { get; set; }
			public string PhoneNumber { get; set; }
		}

		private async Task LoadAsync(AppUser user)
		{
			var userName = await _userManager.GetUserNameAsync(user);
			var phoneNumber = _userManager.GetUserAsync(User).Result.PhoneNumber;

			var userId = _userManager.GetUserAsync(User).Result.Id;
			var selectedUser = await _userManager.FindByIdAsync(userId);
			var roles = await _userManager.GetRolesAsync(selectedUser);
			var firstName = _userManager.GetUserAsync(User).Result.FirstName;
			var lastName = _userManager.GetUserAsync(User).Result.LastName;
			Username = userName;
			UserRoleList = roles;
			Input = new InputModel
			{
				FirstName = firstName,
				LastName = lastName,
				PhoneNumber = phoneNumber
			};
		}



		public async Task<IActionResult> OnGetAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			//var userRole = await _userManager.GetRolesAsync(user);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			await LoadAsync(user);
			return Page();
		}


		public async Task<IActionResult> OnPostAsync()
		{
			var user = await _userManager.FindByNameAsync(User.Identity.Name);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			if (!ModelState.IsValid)
			{
				await LoadAsync(user);
				return Page();
			}


			if (Input.PhoneNumber != user.PhoneNumber)
			{
				user.PhoneNumber = Input.PhoneNumber;
				IdentityResult result = await _userManager.UpdateAsync(user);
				if (!result.Succeeded)
				{
					StatusMessage = "Unexpected error when trying to set phone number.";
					return RedirectToPage();
				}
				else
				{
					StatusMessage = "Profil bilgileri güncellendi.";
					await _userManager.UpdateSecurityStampAsync(user);
				}
			}

			await _signInManager.RefreshSignInAsync(user);
			return RedirectToPage();
		}
	}
}
