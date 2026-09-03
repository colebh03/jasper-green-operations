using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace JasperGreen.Models
{    
    public class CrewViewModel
    {        
        public List<Employee> Employees { get; set; } = new List<Employee>();
       
        public Crew Crew { get; set; } = new Crew();

        // Indicates whether the shared form is in Add or Edit mode
        public string Action { get; set; } = string.Empty;
    }
}