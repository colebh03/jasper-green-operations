using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace JasperGreen.Models
{   
    public class ServiceViewModel
    {        
        public List<Customer> Customers { get; set; } = new List<Customer>();
       
        public List<Crew> Crews { get; set; } = new List<Crew>();
        
        public List<Property> Properties { get; set; } = new List<Property>();
       
        public Service Service { get; set; } = new Service();

        // Indicates whether the shared form is in Add or Edit mode
        public string Action { get; set; } = string.Empty;
    }
}