using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JasperGreen.Models
{    
    public class Payment
    {        
        [Key]
        public int Payment_ID { get; set; }

        [Required(ErrorMessage = "Please select a service.")]
        public int Service_ID { get; set; }

        [Required(ErrorMessage = "Please enter a payment amount.")]
        [Range(0.01, 999999.99, ErrorMessage = "Payment amount must be greater than zero and less than $1,000,000.")]
        [Display(Name = "payment amount")]
        public decimal Payment_Amount { get; set; }

        [Required(ErrorMessage = "Please enter a payment date.")]
        public DateTime Payment_Date { get; set; } = DateTime.Now; 

        [Required(ErrorMessage = "Please select a payment method.")]
        public string Payment_Method { get; set; } = "";

        [ForeignKey("Service_ID")]
        [ValidateNever]
        public Service Service { get; set; } = null!;        
    }
}
