using System.ComponentModel.DataAnnotations;

namespace Web.Models.UserAuthentication
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="First name not provided")]
        public string FirstName { get; set; }
        [Required(ErrorMessage ="Last name not provided")]
        public string LastName { get; set; }
        [Required(ErrorMessage ="Phone Number not provided")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public IFormFile ImageFormFile { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Your password and confirm password do not match")]
        public string ConfirmPassword { get; set; }

    }
}
