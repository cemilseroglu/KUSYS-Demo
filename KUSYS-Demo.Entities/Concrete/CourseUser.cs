using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KUSYS_Demo.Entities.Abstract;

namespace KUSYS_Demo.Entities.Concrete
{
	public class CourseUser : BaseEntity,IEntity
	{
		[ForeignKey("CourseId")]
		public string CourseId { get; set; }
		[ForeignKey("UserId")]
		public string UserId { get; set; }
		public Course Course { get; set; }
		public AppUser User { get; set; }
	}
}
