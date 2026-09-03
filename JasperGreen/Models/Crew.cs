using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace JasperGreen.Models
{    
    public class Crew : IValidatableObject
    {        
        [Key]
        public int Crew_ID { get; set; }
        
        [Required(ErrorMessage = "Please select a foreman.")]        
        public int Crew_Foreman { get; set; }

        [Required(ErrorMessage = "Please select a member #1.")]
        public int Crew_Member_1 { get; set; }

        [Required(ErrorMessage = "Please select a member #2.")]
        public int Crew_Member_2 { get; set; }

        [ValidateNever]
        [ForeignKey("Crew_Foreman")]
        public Employee Foreman { get; set; }

        [ValidateNever]
        [ForeignKey("Crew_Member_1")]
        public Employee CrewMember1 { get; set; }

        [ValidateNever]
        [ForeignKey("Crew_Member_2")]
        public Employee CrewMember2 { get; set; }

        public ICollection<Service> Services { get; set; } = new List<Service>();

        public string CrewDisplayShort => $"{Foreman?.Emp_Last_Name}";

        public string CrewDisplayLong => $"{Foreman?.Emp_Last_Name}, {CrewMember1?.Emp_Last_Name}, {CrewMember2?.Emp_Last_Name}";

        // Prevents the same employee from filling multiple positions on a crew
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {            
            var ids = new[] { Crew_Foreman, Crew_Member_1, Crew_Member_2 };

            if (ids.Distinct().Count() != 3)
            {
                yield return new ValidationResult(
                    "Crew must contain three distinct employees."
                );
            }
        }
    }
}
