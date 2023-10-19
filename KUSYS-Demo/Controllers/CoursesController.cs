using AutoMapper;
using KUSYS_Demo.Data;
using KUSYS_Demo.Data.Services.Abstract;
using KUSYS_Demo.Entities.Abstract;
using KUSYS_Demo.Entities.Concrete;
using KUSYS_Demo.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace KUSYS_Demo.Controllers
{
	public class CoursesController : Controller
	{
		private UserManager<AppUser> _userManager { get; }
		private RoleManager<AppRole> _roleManager { get; }
		private readonly ApplicationDbContext _context;
		private readonly ICourseService _courseService;
		private readonly IMapper _mapper;

		public CoursesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ApplicationDbContext context, IToastNotification toastNotification, ICourseService courseService, IMapper mapper)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
			_courseService = courseService;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
		{
			IEnumerable<Course> courses = await _courseService.GetAll();
			return View(courses);
		}

		public IActionResult CreateCourse()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> CreateCourse(CourseCreateViewModel model)
		{
			if (ModelState.IsValid)
			{
				Course entity = new Course();
				_mapper.Map<CourseCreateViewModel, Course>(model, entity);
				await _courseService.Add(entity);
				return RedirectToAction("Index", "Courses");
			}
			return View();
		}

		public async Task<IActionResult> CourseDelete(string id)
		{
			var courseSelector = _courseService.GetById(id).Result;
			if (courseSelector != null)
			{
				await _courseService.Remove(id);
				return RedirectToAction("Index", "Courses");
			}
			else
			{
				return RedirectToAction("Index", "Courses");
			}
		}

		public async Task<IActionResult> CourseUpdate(string id)
		{
			Course courseSelector = await _courseService.GetById(id);
			if (courseSelector != null)
			{
				return View(courseSelector.Adapt<CourseCreateViewModel>());
			}
			return RedirectToAction("Courses");
		}

		[HttpPost]
		public IActionResult CourseUpdate(CourseCreateViewModel courseViewModel)
		{
			var courseSelector = _courseService.GetById(courseViewModel.Id).Result;
			if (courseSelector != null)
			{
				_mapper.Map<CourseCreateViewModel, Course>(courseViewModel, courseSelector);
				var result = _courseService.Update(courseViewModel.Id, courseSelector);
				if (result.IsCompletedSuccessfully)
				{
					return RedirectToAction("CourseUpdate", "Courses", new { id = courseSelector.Id });
				}
				else
				{
					return RedirectToAction("Index", "Courses");
				}
			}
			else
			{
				return RedirectToAction("Index", "Courses");
			}
		}
	}
}
