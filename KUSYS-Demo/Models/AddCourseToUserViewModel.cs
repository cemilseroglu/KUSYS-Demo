using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using KUSYS_Demo.Entities.Concrete;

namespace KUSYS_Demo.Models
{
    public class AddCourseToUserViewModel
    {
        public IEnumerable<Course>? OwnableCourseList { get; set; }
        public string CourseId;
    }
}
