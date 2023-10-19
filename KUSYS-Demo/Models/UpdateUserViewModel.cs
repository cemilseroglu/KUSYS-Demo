using System.ComponentModel.DataAnnotations;

namespace KUSYS_Demo.Models
{
    public class UpdateUserViewModel
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        [StringLength(50, ErrorMessage = "Adınız 2 karakterden az, 50 karakterden fazla olamaz.", MinimumLength = 2)]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        [StringLength(50, ErrorMessage = "Soyadınız 2 karakterden az, 50 karakterden fazla olamaz.", MinimumLength = 2)]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Display(Name = "Telefon Numarası")]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        [StringLength(10, ErrorMessage = "Telefon Numaranızı başında 0 olmadan, 10 hane olacak şekilde giriniz.", MinimumLength = 10)]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Geçerli bir Telefon Numarası Girin")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Geçerli bir Telefon Numarası Girin")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


    }
}
