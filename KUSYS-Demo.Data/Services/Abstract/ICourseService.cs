using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KUSYS_Demo.Data.Repositories.Abstract;
using KUSYS_Demo.Entities.Concrete;

namespace KUSYS_Demo.Data.Services.Abstract
{
	public interface ICourseService : IRepository<Course>
	{

	}
}
