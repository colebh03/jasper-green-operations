/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      Customer.cs (Customer Model Class)

PURPOSE:      Defines the Customer entity and its attributes for the Jasper Green
              application, including validation rules and relationships to
              associated properties and service records. 

INPUT:        Customer data entered by the user or seeded into the database,
              including name, address, contact information, and related entities.

PROCESS:      Customer data entered by the user or seeded into the database,
              including name, address, contact information, and related entities.

OUTPUT:       Applies data annotations to enforce validation rules and maps
              relationships between Customer and related entities using
              navigation properties.

HONOR CODE:   On my honor, as an Aggie, I have neither given nor received
              unauthorized aid on this academic work.
============================================================================
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JasperGreen.Models
{
    // Customer entity representing an individual customer in the system
    public class Customer
    {
        //Primary key
        [Key]
        public int Cust_ID { get; set; }

        //Required full name
        [Required(ErrorMessage = "Please enter a full name.")]
        [StringLength(70, ErrorMessage = "Name may not exceed 70 characters.")]
        public string Cust_Name { get; set; } = string.Empty;

        //Required billing address
        [Required(ErrorMessage = "Please enter a billing address.")]
        [StringLength(50, ErrorMessage = "Billing address may not exceed 50 characters.")]
        public string Cust_Billing_Address { get; set; } = string.Empty;

        //Required billing city
        [Required(ErrorMessage = "Please enter a billing city.")]
        [StringLength(50, ErrorMessage = "Billing city may not exceed 50 characters.")]
        public string Cust_Billing_City { get; set; } = string.Empty;

        //Required billing state
        [Required(ErrorMessage = "Please enter a billing state.")]
        [StringLength(50, ErrorMessage = "Billing state may not exceed 50 characters.")]
        public string Cust_Billing_State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a billing ZIP code.")]
        [RegularExpression(@"^\d{5}(-\d{4})?$",
        ErrorMessage = "ZIP must be 5 digits or 5+4 format (e.g., 77840 or 77840-1234).")]
        public string Cust_Billing_Zip { get; set; } = string.Empty;

        //Required phone number
        [Required(ErrorMessage = "Please enter a phone number.")]
        [RegularExpression(@"^\d{10}$",
        ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string Cust_Phone { get; set; } = string.Empty;

        //Optional email address
        [StringLength(50, ErrorMessage = "Email address may not exceed 50 characters.")]
        [DataType(DataType.EmailAddress)]        
        public string? Cust_Email { get; set; }

        //Navigation Collection
        public ICollection<Property> Properties { get; set; } = new List<Property>(); // skip navigation property for one-to-many relationship, see textbook

        //Navigation Collection
        [InverseProperty("Customer")]
        public ICollection<Service> Services { get; set; } = new List<Service>();  // skip navigation property for one-to-many relationship, see textbook
    }
}
