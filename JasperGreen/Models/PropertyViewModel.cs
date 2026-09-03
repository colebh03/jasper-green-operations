using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace JasperGreen.Models
{    
    public class PropertyViewModel
    {        
        public List<Customer> Customers { get; set; } = new List<Customer>();
        
        public Property Property { get; set; } = new Property();

        // Indicates whether the shared form is in Add or Edit mode
        public string Action { get; set; } = string.Empty;

        // For populating state dropdown box
        public List<SelectListItem> States { get; set; } = new();
    }
}