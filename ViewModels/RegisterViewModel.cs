using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp1.ViewModels
{
    public class RegisterViewModel
    {

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password mismatch")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }
    }
}