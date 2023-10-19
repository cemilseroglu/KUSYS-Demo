using System.ComponentModel.DataAnnotations;

namespace KUSYS_Demo.Models
{
    public class RoleAssignViewModel
    {
        public string RoleId { get; set; }
        [Display(Name = "Role ismi")]
        public string RoleName { get; set; }
        public bool Exist { get; set; }
    }
}
