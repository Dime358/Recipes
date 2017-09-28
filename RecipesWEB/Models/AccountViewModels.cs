using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipesWEB.Models
{
    public class ExternalLoginConfirmationViewModel
    {
         [Required]
         [Display(Name = "Email")]
         public string Email { get; set; }

        
        [Required(ErrorMessage = "Внесете корисничко име")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

    
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessage = "Внесете валидна email адреса")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        //[Required]
        //[Display(Name = "Email")]
        //[EmailAddress]
        //public string Email { get; set; }


        [Required(ErrorMessage = "Внесете корисничко име")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Внесете лозинка")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Внесете валидна email адреса")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Внесете корисничко име")]
        [Display(Name = "Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Внесете лозинка")]
        [StringLength(100, ErrorMessage = "лозинката мора да биде долга барем 6 карактери.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "лозинката и лозинката за потврда не се сопфаѓаат.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterViewModelAdmin
    {
        [Required(ErrorMessage = "Внесете улога")]
        [Display(Name = "UserRoles")]
        public string UserRoles { get; set; }

        [Required(ErrorMessage = "Внесете валидна email адреса")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Внесете корисничко име")]
        [Display(Name = "Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Внесете лозинка")]
        [StringLength(100, ErrorMessage = "лозинката мора да биде долга барем 6 карактери.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "лозинката и лозинката за потврда не се сопфаѓаат.")]
        public string ConfirmPassword { get; set; }
    }


    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Внесете валидна email адреса")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Внесете лозинка")]
        [StringLength(100, ErrorMessage = "лозинката мора да биде долга барем 6 карактери.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "лозинката и лозинката за потврда не се сопфаѓаат.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Внесете валидна email адреса")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
