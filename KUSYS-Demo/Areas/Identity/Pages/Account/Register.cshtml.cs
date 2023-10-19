// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using KUSYS_Demo.Data.Utilities.Config;
using KUSYS_Demo.Entities.Concrete;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace KUSYS_Demo.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStore<AppUser> _userStore;
        //private readonly IUserEmailStore<AppUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<AppUser> userManager,
            IUserStore<AppUser> userStore,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            //_emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }


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
		public string ReturnUrl { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public IList<AuthenticationScheme> ExternalLogins { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public class InputModel
		{
			[Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
			[StringLength(50, ErrorMessage = "Adınız 2 karakterden az, 50 karakterden fazla olamaz.", MinimumLength = 2)]
			[Display(Name = "Ad")]
			public string FirstName { get; set; }

			[Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
			[StringLength(50, ErrorMessage = "Soyadınız 2 karakterden az, 50 karakterden fazla olamaz.", MinimumLength = 2)]
			[Display(Name = "Soyad")]
			public string LastName { get; set; }

			[Display(Name = "Doğum Tarihi")]
			[Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
			public DateTime BirthDate { get; set; }

			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
			[EmailAddress(ErrorMessage = "Geçerli bir E-Posta Adresi Girin")]
			[Display(Name = "Email")]
			public string Email { get; set; }

			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
			[StringLength(100, ErrorMessage = "Şifre en az {2} ve en fazla {1} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; }

			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[DataType(DataType.Password)]
			[Display(Name = "Confirm password")]
			[Compare("Password", ErrorMessage = "Parolalarlar birbiri ile uyuşmuyor!")]
			public string ConfirmPassword { get; set; }
		}


		public async Task OnGetAsync(string returnUrl = null)
		{
			ReturnUrl = returnUrl;
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			Random random = new Random();
			returnUrl ??= Url.Content("~/");
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
			if (ModelState.IsValid)
			{
				var user = CreateUser();

				user.FirstName = Input.FirstName;
				user.LastName = Input.LastName;
				user.BirthDate = Input.BirthDate;
				user.StudentNumber = random.Next(100000, 1000000).ToString();
				user.EmailConfirmed = true;
				user.Email = Input.Email;

				while (_userManager.Users.Any(x => x.StudentNumber == user.StudentNumber))
				{
					user.StudentNumber = random.Next(100000, 1000000).ToString();
				}

				await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
				var result = await _userManager.
					CreateAsync(user, Input.Password);
				await _userManager.AddToRoleAsync(user, SystemConfig.StudentRole);

				return RedirectToAction("Index", "Home");
			}

			return RedirectToAction("Index","Home");
		}

		private AppUser CreateUser()
		{
			RoleManager<AppRole> roleManager;
			try
			{
				return Activator.CreateInstance<AppUser>();
			}
			catch
			{
				throw new InvalidOperationException($"Can't create an instance of '{nameof(AppUser)}'. " +
					$"Ensure that '{nameof(AppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
					$"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
			}
		}

		private IUserEmailStore<AppUser> GetEmailStore()
		{
			if (!_userManager.SupportsUserEmail)
			{
				throw new NotSupportedException("The default UI requires a user store with email support.");
			}
			return (IUserEmailStore<AppUser>)_userStore;
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				string errorMessage = error.Description;
				if (errorMessage.EndsWith("is already taken."))
					errorMessage = "Bu e-posta adresine tanımlı bir üyelik mevcut.Lütfen farklı bir e-posta adresi ile deneyiniz.";
				ModelState.AddModelError("", errorMessage);
			}
		}
	}
}

