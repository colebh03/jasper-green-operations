/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      Property.cs (Property Model Class)

PURPOSE:      Defines the Property entity and its attributes for the Jasper Green
              application, including validation rules and relationships to
              associated properties and service records. 

INPUT:        Property data entered by the user or seeded into the database,
              including address, city, state, zip, service fee, related entities.

PROCESS:      Property data entered by the user or seeded into the database,
              including address, city, state, zip, service fee, related entities.

OUTPUT:       Applies data annotations to enforce validation rules and maps
              relationships between Property and related entities using
              navigation properties.

HONOR CODE:   On my honor, as an Aggie, I have neither given nor received
              unauthorized aid on this academic work.
============================================================================
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace JasperGreen.Models
{
    // Property entity representing an individual property in the system
    public class Property
    {
        //Primary key
        [Key]
        public int Property_ID { get; set; }

        //Required foreign key referencing Customer
        [Required(ErrorMessage = "Please select a customer.")]    
        public int Cust_ID { get; set; }

        //Required address
        [Required(ErrorMessage = "Please enter an address.")]
        [StringLength(50, ErrorMessage = "Address may not exceed 50 characters.")]
        public string Property_Address { get; set; } = string.Empty;

        //Required city
        [Required(ErrorMessage = "Please enter a city.")]
        [StringLength(50, ErrorMessage = "City may not exceed 50 characters.")]
        public string Property_City { get; set; } = string.Empty;

        //Required state
        [Required(ErrorMessage = "Please enter a state.")]
        [StringLength(50, ErrorMessage = "State may not exceed 2 characters.")] //we might want to add a state model for a dropdown in future!
        public string Property_State { get; set; } = string.Empty;

        //Required ZIP code
        [Required(ErrorMessage = "Please enter a ZIP code.")]
        [StringLength(20, ErrorMessage = "ZIP code may not exceed 50 characters.")]
        public string Property_ZIP { get; set; } = string.Empty;

        //Required service fee
        [Required(ErrorMessage = "Please enter a service fee.")]
        [Range(0.01, 999999.99, ErrorMessage = "Service fee must be greater than zero and less than $1,000,000.")] //limited to $999,999.99
        public decimal Property_Service_Fee { get; set; }        

        [ForeignKey("Cust_ID")]
        [ValidateNever]
        public Customer Customer { get; set; } = null!;

        //Navigation Collection
        public ICollection<Service> Services { get; set; } = new List<Service>(); // skip navigation property for many-to-many relationship, see textbook

        // Read-only computed property returning the property's full address
        public string Property_Full_Address => $"{Property_Address}, {Property_City}, {Property_State} {Property_ZIP}";

    }
}

