using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace JasperGreen.Models
{
    public class UserAddEditViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Please enter a username.")]
        public string Username { get; set; } = "";

        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(
            "Password",
            ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; } = "";
    }
}