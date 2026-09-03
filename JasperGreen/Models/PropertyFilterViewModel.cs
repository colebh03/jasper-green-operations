using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace JasperGreen.Models
{
    public class PropertyFilterViewModel
    {        
        [Required(ErrorMessage = "Please select a property.")]
        public int? Property_ID { get; set; }

        // Available properties used to populate the dropdown filter
        public List<Property> Properties { get; set; } = new();
    }
}