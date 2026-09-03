using System.ComponentModel.DataAnnotations;

namespace JasperGreen.Models
{
    public class CustomerFilterViewModel
    {        
        [Required(ErrorMessage = "Please select a customer.")]

        public int? Cust_ID { get; set; }

        // Available customers used to populate the dropdown filter
        public List<Customer> Customers { get; set; } = new();
    }
}