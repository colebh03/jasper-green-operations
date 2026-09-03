using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JasperGreen.Models
{    
    public class Service
    {        
        [Key]
        public int Service_ID { get; set; }

        [Required(ErrorMessage = "Please select a crew.")]
        public int Crew_ID { get; set; }

        [Required(ErrorMessage = "Please select a customer.")]
        public int Cust_ID { get; set; }

        [Required(ErrorMessage = "Please select a property.")]
        public int Property_ID { get; set; }
    
        [Required(ErrorMessage = "Please enter a service date.")]
        public DateTime Service_Date { get; set; } = DateTime.Now; //will default populate with today's date but can be overridden
        
        [Required(ErrorMessage = "Please enter a service fee.")]
        [Range(0.01, 999999.99, ErrorMessage = "Service fee must be greater than zero and less than $1,000,000.")]
        public decimal Service_Fee { get; set; }       

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
