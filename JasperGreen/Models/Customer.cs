using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JasperGreen.Models
{    
    public class Customer
    {        
        [Key]
        public int Cust_ID { get; set; }

        [Required(ErrorMessage = "Please enter a full name.")]
        [StringLength(70, ErrorMessage = "Name may not exceed 70 characters.")]
        public string Cust_Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a billing address.")]
        [StringLength(50, ErrorMessage = "Billing address may not exceed 50 characters.")]
        public string Cust_Billing_Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a billing city.")]
        [StringLength(50, ErrorMessage = "Billing city may not exceed 50 characters.")]
        public string Cust_Billing_City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a billing state.")]
        [StringLength(50, ErrorMessage = "Billing state may not exceed 50 characters.")]
        public string Cust_Billing_State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a billing ZIP code.")]
        [RegularExpression(@"^\d{5}(-\d{4})?$",
        ErrorMessage = "ZIP must be 5 digits or 5+4 format (e.g., 77840 or 77840-1234).")]
        public string Cust_Billing_Zip { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a phone number.")]
        [RegularExpression(@"^\d{10}$",
        ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string Cust_Phone { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Email address may not exceed 50 characters.")]
        [DataType(DataType.EmailAddress)]        
        public string? Cust_Email { get; set; }

        // Navigation properties for the customer's associated properties and service history
        public ICollection<Property> Properties { get; set; } = new List<Property>(); 

        [InverseProperty("Customer")]
        public ICollection<Service> Services { get; set; } = new List<Service>();
    }
}