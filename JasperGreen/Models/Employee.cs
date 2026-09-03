using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace JasperGreen.Models
{   
    public class Employee
    {        
        [Key]
        public int Emp_ID { get; set; }

        [Required(ErrorMessage = "Please enter a first name.")]
        [StringLength(50, ErrorMessage = "First name may not exceed 50 characters.")]
        public string Emp_First_Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a last name.")]
        [StringLength(50, ErrorMessage = "Last name may not exceed 50 characters.")]
        public string Emp_Last_Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a social security number.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "SSN must be exactly 9 digits with no dashes.")]
        public string Emp_SSN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a job title.")]
        [StringLength(50, ErrorMessage = "Job title may not exceed 50 characters.")]
        public string Emp_Job_Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a hire date.")]
        public DateOnly Emp_Hire_Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Required(ErrorMessage = "Please enter an hourly rate.")]
        [Range(0.01, 999.99, ErrorMessage = "Hourly rate must be greater than zero and less than $1,000.")]
        public decimal Emp_Hourly_Rate { get; set; }

        public string Emp_Full_Name => $"{Emp_First_Name} {Emp_Last_Name}";s
    }
}
