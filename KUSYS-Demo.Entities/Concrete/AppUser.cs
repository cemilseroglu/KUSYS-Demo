using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace KUSYS_Demo.Entities.Concrete
{
	public class AppUser : IdentityUser
	{
		public string StudentNumber { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime BirthDate { get; set; }
		public ICollection<CourseUser>? CourseUsers { get; set; }
	}
}
