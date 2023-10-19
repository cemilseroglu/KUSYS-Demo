using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KUSYS_Demo.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using static System.Formats.Asn1.AsnWriter;

namespace KUSYS_Demo.Data.Utilities.Config
{
	/// <summary>
	/// This class is configures system for first use. 
	/// 
	/// Adds roles from SystemConfig class
	/// Adds system admin from SystemConfig class
	/// 
	/// 
	/// </summary>
	public static class SeedData
	{
		public static void SeedDbData(IServiceProvider serviceProvider)
		{
			SeedRoles(serviceProvider);
			SeedUsers(serviceProvider);
			SeedCourses(serviceProvider);
		}

		public static void SeedRoles(IServiceProvider serviceProvider)
		{
			using (var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>())
			{
				if (roleManager.RoleExistsAsync(SystemConfig.AdminRole).Result)
				{ }
				else
				{
					roleManager.CreateAsync(new AppRole(SystemConfig.AdminRole)).Wait();
					roleManager.CreateAsync(new AppRole(SystemConfig.StudentRole)).Wait();
				}
			}
		}

		private static void SeedUsers(IServiceProvider serviceProvider)
		{
			using (var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>())
			{
				var user = new AppUser
				{
					StudentNumber = "000000",
					Email = SystemConfig.SystemAdmin,
					FirstName = "System Admin",
					LastName = "Admin",
					EmailConfirmed = true,
					UserName = SystemConfig.SystemAdmin,
				};
				if (userManager.FindByEmailAsync(user.Email).Result != null)
				{ }
				else
				{
					var result = userManager.CreateAsync(user, "123123").Result;
					if (result.Succeeded)
					{
						userManager.AddToRoleAsync(user, SystemConfig.StudentRole).Wait();
						userManager.AddToRoleAsync(user, SystemConfig.AdminRole).Wait();
					}
				}
			}
		}
		
		private static void SeedCourses(IServiceProvider serviceProvider)
		{
			using (var scope = serviceProvider.CreateScope())
			{
				var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
				using var dbContext = dbContextFactory.CreateDbContext();
				List<Course> course = new List<Course>()
				{
					new Course(){Id = "CSI101",CourseName = "Introduction to Computer Science"},
					new Course(){Id = "CSI102", CourseName = "Algorithms"},
					new Course(){Id = "MAT101", CourseName = "Calculus"},
					new Course(){Id = "PHY101", CourseName = "Physics"}
				};
				if (dbContext.Courses.FirstOrDefault(x=>x.Id == course.First().Id) != null)
				{ }
				else
				{
					dbContext.AddRange(course);
					dbContext.SaveChanges();
				}
			}
		}
	}
}

