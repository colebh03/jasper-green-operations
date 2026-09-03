using System.ComponentModel.DataAnnotations;

namespace JasperGreen.Models
{
    public class ChangePasswordViewModel
    {
        // Display-only username for the currently authenticated user
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Please enter your current password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string OldPassword { get; set; } = "";

        [Required(ErrorMessage = "Please enter your new password.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; } = "";

        [Required(ErrorMessage = "Please confirm your new password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(
            "NewPassword",
            ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; } = "";
    }
}