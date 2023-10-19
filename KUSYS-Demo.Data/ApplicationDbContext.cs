using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using KUSYS_Demo.Entities.Abstract;
using KUSYS_Demo.Entities.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KUSYS_Demo.Data
{
	public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, string>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
			
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);


			builder.Entity<AppUser>().Ignore(e => e.TwoFactorEnabled);
			builder.Entity<AppUser>().Ignore(e => e.PhoneNumberConfirmed);

			builder.Entity<Course>().HasKey(e => e.Id);
			builder.Entity<Course>().Property(e => e.Id).ValueGeneratedNever();
		}


		/// <summary>
		/// Overrided SaveChanges
		/// </summary>
		/// <returns></returns>
		public override int SaveChanges()
		{
			var entries = ChangeTracker
				.Entries()
				.Where(e => e.Entity is BaseEntity && (
						e.State == EntityState.Added
						|| e.State == EntityState.Modified));

			foreach (var entityEntry in entries)
			{
				((BaseEntity)entityEntry.Entity).Updated = DateTime.Now;

				if (entityEntry.State == EntityState.Added)
				{
					((BaseEntity)entityEntry.Entity).Created = DateTime.Now;
				}
			}
			return base.SaveChanges();
		}

		public DbSet<AppUser> Users { get; set; }
		public DbSet<AppRole> Roles { get; set; }
		public DbSet<Course> Courses { get; set; }
		public DbSet<CourseUser> CourseUser { get; set; }


	}
}
