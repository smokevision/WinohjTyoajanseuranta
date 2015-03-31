using System.ComponentModel.DataAnnotations;

namespace TyoaikaApp.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string Action { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Käyttäjätunnus")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Salasana")]
        public string Password { get; set; }

        [Display(Name = "Muista minut?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Sähköposti")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Salasanan täytyy olla vähintään {2} merkkiä pitkä.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Salasana")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Salasana uudelleen")]
        [Compare("Password", ErrorMessage = "Salasanat eivät täsmää.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Käyttäjänimi")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Etunimi")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Sukunimi")]
        public string LastName { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
