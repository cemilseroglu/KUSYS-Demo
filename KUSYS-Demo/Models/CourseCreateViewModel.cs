using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace KUSYS_Demo.Models
{
    public class CourseCreateViewModel
    {
        [Required]
        public string Id { get; set; }
		[Required]
		public string CourseName { get; set; }
    }
}
