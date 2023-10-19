using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KUSYS_Demo.Entities.Abstract;

namespace KUSYS_Demo.Entities.Concrete
{
	public class Course : BaseEntity
	{
		public string CourseName { get; set; }
		public ICollection<CourseUser> CourseUsers  { get; set; }
	}
}
