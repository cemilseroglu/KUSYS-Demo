using System.ComponentModel.DataAnnotations;

namespace KUSYS_Demo.Models
{
    public class RoleViewModel

    {
        [Display(Name = "Role ismi")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Role ismi gereklidir")]
        public string Name { get; set; }

        public string Id { get; set; }
    }
}
