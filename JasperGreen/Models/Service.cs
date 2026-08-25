/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      Service.cs (Service Model Class)

PURPOSE:      Defines the Service entity and its attributes for the Jasper Green
              application, including validation rules and relationships to
              associated properties and service records. 

INPUT:        Service data entered by the user or seeded into the database,
              including date, fee, payment, and related entities

PROCESS:      Service data entered by the user or seeded into the database,
              including date, fee, payment, and related entities

OUTPUT:       Applies data annotations to enforce validation rules and maps
              relationships between Service and related entities using
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
    // Service entity representing an individual service in the system
    public class Service
    {
        //Primary key
        [Key]
        public int Service_ID { get; set; }

        //Required foreign key referencing Crew
        [Required(ErrorMessage = "Please select a crew.")]
        public int Crew_ID { get; set; }

        //Required foreign key referencing Customer
        [Required(ErrorMessage = "Please select a customer.")]
        public int Cust_ID { get; set; }

        //Required foreign key referencing Property
        [Required(ErrorMessage = "Please select a property.")]
        public int Property_ID { get; set; }

        //Required service date      
        [Required(ErrorMessage = "Please enter a service date.")]
        public DateTime Service_Date { get; set; } = DateTime.Now; //will default populate with today's date but can be overridden

        //Required service fee
        [Required(ErrorMessage = "Please enter a service fee.")]
        [Range(0.01, 999999.99, ErrorMessage = "Service fee must be greater than zero and less than $1,000,000.")] //limited to $999,999.99
        public decimal Service_Fee { get; set; }

        ////Optional foreign key referencing Payment (Not required b/c customers can pay later in time)        
        //public int? Payment_ID { get; set; } 

        [ForeignKey("Crew_ID")]
        [ValidateNever]
        public Crew Crew { get; set; } = null!;     

        [ForeignKey("Cust_ID")]
        [ValidateNever]
        public Customer Customer { get; set; } = null!;       

        [ForeignKey("Property_ID")]
        [ValidateNever]
        public Property Property { get; set; } = null!;

        [ValidateNever]
        public Payment? Payment { get; set; }

    }
}
