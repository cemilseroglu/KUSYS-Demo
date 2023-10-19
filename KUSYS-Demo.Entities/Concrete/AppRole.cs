using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace KUSYS_Demo.Entities.Concrete
{
	public class AppRole : IdentityRole
	{
		public AppRole() : base()
		{
			base.Id = Guid.NewGuid().ToString();
		}

		public AppRole(string roleName) : base()
		{
			base.Name = roleName;
		}

		//Properties
	}
}
