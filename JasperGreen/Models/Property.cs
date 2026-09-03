using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace JasperGreen.Models
{    
    public class Property
    {        
        [Key]
        public int Property_ID { get; set; }

        [Required(ErrorMessage = "Please select a customer.")]    
        public int Cust_ID { get; set; }

        [Required(ErrorMessage = "Please enter an address.")]
        [StringLength(50, ErrorMessage = "Address may not exceed 50 characters.")]
        public string Property_Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a city.")]
        [StringLength(50, ErrorMessage = "City may not exceed 50 characters.")]
        public string Property_City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a state.")]
        [StringLength(50, ErrorMessage = "State may not exceed 2 characters.")]
        public string Property_State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a ZIP code.")]
        [StringLength(20, ErrorMessage = "ZIP code may not exceed 50 characters.")]
        public string Property_ZIP { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a service fee.")]
        [Range(0.01, 999999.99, ErrorMessage = "Service fee must be greater than zero and less than $1,000,000.")]
        public decimal Property_Service_Fee { get; set; }        

        [ForeignKey("Cust_ID")]
        [ValidateNever]
        public Customer Customer { get; set; } = null!;

        //Navigation Collection
        public ICollection<Service> Services { get; set; } = new List<Service>();
        
        public string Property_Full_Address => $"{Property_Address}, {Property_City}, {Property_State} {Property_ZIP}";
    }
}

