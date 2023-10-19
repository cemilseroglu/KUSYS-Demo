using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KUSYS_Demo.Data.Services.Abstract;
using KUSYS_Demo.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace KUSYS_Demo.Data.Services.Concrete
{
	public class UserService : IUserService
	{
		private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IDistributedCache _distributedCache;
		private readonly IEmailSender _emailSender;
		private readonly IMapper _mapper;



		private readonly RoleManager<AppRole> _roleManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly IUserStore<AppUser> _userStore;

		public UserService(
			IUserStore<AppUser> userStore,
			RoleManager<AppRole> roleManager,
			UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager,
			IDbContextFactory<ApplicationDbContext> contextFactory,
			IHttpContextAccessor httpContextAccessor,
			IDistributedCache distributedCache,
			IEmailSender emailSender,
			IMapper mapper
			)
		{
			_userStore = userStore;
			_contextFactory = contextFactory;
			_distributedCache = distributedCache;
			_httpContextAccessor = httpContextAccessor;
			_userManager = userManager;
			_emailSender = emailSender;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_mapper = mapper;
		}
	}
}
