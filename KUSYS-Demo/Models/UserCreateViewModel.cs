using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KUSYS_Demo.Models
{
    public class UserCreateViewModel
    {
        [Required(ErrorMessage ="Kullanıcının ad bilgisinin girilmesi zorunldur!")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Kullanıcının soyad bilgisinin girilmesi zorunldur!")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Kullanıcının e-posta bilgisinin girilmesi zorunldur!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Kullanıcının doğum tarihi girmesi zorunldur!")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Kullanıcının parola bilgisinin girilmesi zorunldur!")]
        public string Password { get; set; }
        [NotMapped]
        [Compare("Password",ErrorMessage ="Parolalar birbiri ile uyuşmuyor!")]
        public string ConfirmPassword { get; set; }
    }
}
