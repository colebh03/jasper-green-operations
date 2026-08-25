/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      Payment.cs (Payment Model Class)

PURPOSE:      Defines the Payment entity and its attributes for the Jasper Green
              application, including validation rules and relationships to
              associated properties and service records. 

INPUT:        Payment data entered by the user or seeded into the database,
              including payment date, amount, and related entities.

PROCESS:      Payment data entered by the user or seeded into the database,
              including payment date, amount, and related entities.

OUTPUT:       Applies data annotations to enforce validation rules and maps
              relationships between Payment and related entities using
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
    // Payment entity representing an individual provided payment in the system
    public class Payment
    {
        //Primary key
        [Key]
        public int Payment_ID { get; set; }

        //Required foreign key referencing Service
        [Required(ErrorMessage = "Please select a service.")]
        public int Service_ID { get; set; }

        //Required payment amount
        [Required(ErrorMessage = "Please enter a payment amount.")]
        [Range(0.01, 999999.99, ErrorMessage = "Payment amount must be greater than zero and less than $1,000,000.")] //limited to $999,999.99, WHAT ABOUT REFUNDS?
        [Display(Name = "payment amount")]
        public decimal Payment_Amount { get; set; }

        //Required payment process date
        [Required(ErrorMessage = "Please enter a payment date.")]
        public DateTime Payment_Date { get; set; } = DateTime.Now; //will default populate with today's date but can be overridden

        //Required payment method
        [Required(ErrorMessage = "Please select a payment method.")]
        public string Payment_Method { get; set; } = "";

        [ForeignKey("Service_ID")]
        [ValidateNever]
        public Service Service { get; set; } = null!;
        
    }
}
