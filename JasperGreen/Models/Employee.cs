/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      Employee.cs (Employee Model Class)

PURPOSE:      Defines the Employee entity and its attributes for the Jasper Green
              application, including validation rules and relationships to
              associated properties.

INPUT:        Employee data entered by the user or seeded into the database,
              including name, ssn, job title, hire date, and hourly rate.

PROCESS:      Employee data entered by the user or seeded into the database,
              including name, ssn, job title, hire date, and hourly rate.

OUTPUT:       Applies data annotations to enforce validation rules and maps
              relationships between Employee and related entities.

HONOR CODE:   On my honor, as an Aggie, I have neither given nor received
              unauthorized aid on this academic work.
============================================================================
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace JasperGreen.Models
{
    // Employee entity representing an individual employee in the system
    public class Employee
    {
        //Primary key
        [Key]
        public int Emp_ID { get; set; }

        //Required first name
        [Required(ErrorMessage = "Please enter a first name.")]
        [StringLength(50, ErrorMessage = "First name may not exceed 50 characters.")]
        public string Emp_First_Name { get; set; } = string.Empty;

        //Required last name
        [Required(ErrorMessage = "Please enter a last name.")]
        [StringLength(50, ErrorMessage = "Last name may not exceed 50 characters.")]
        public string Emp_Last_Name { get; set; } = string.Empty;

        //Required SSN
        [Required(ErrorMessage = "Please enter a social security number.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "SSN must be exactly 9 digits with no dashes.")]
        public string Emp_SSN { get; set; } = string.Empty;

        //Required job title
        [Required(ErrorMessage = "Please enter a job title.")]
        [StringLength(50, ErrorMessage = "Job title may not exceed 50 characters.")]
        public string Emp_Job_Title { get; set; } = string.Empty;

        //Required hire date
        [Required(ErrorMessage = "Please enter a hire date.")]
        public DateOnly Emp_Hire_Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        //Required hourly rate
        [Required(ErrorMessage = "Please enter an hourly rate.")]
        [Range(0.01, 999.99, ErrorMessage = "Hourly rate must be greater than zero and less than $1,000.")] //limited to $999.99
        public decimal Emp_Hourly_Rate { get; set; }

        public string Emp_Full_Name => $"{Emp_First_Name} {Emp_Last_Name}";

    }
}
