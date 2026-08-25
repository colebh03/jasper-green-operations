/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      Crew.cs (Crew Model Class)

PURPOSE:      Defines the Crew entity and its attributes for the Jasper Green
              application, including validation rules and relationships to
              associated properties.

INPUT:        Crew data entered by the user or seeded into the database,
              including foreman, member1, and member2.

PROCESS:      Crew data entered by the user or seeded into the database,
              including foreman, member1, and member2.

OUTPUT:       Applies data annotations to enforce validation rules and maps
              relationships between Crew and related entities using
              navigation properties.

HONOR CODE:   On my honor, as an Aggie, I have neither given nor received
              unauthorized aid on this academic work.
============================================================================
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace JasperGreen.Models
{
    // Crew entity representing an individual crew in the system
    public class Crew : IValidatableObject
    {
        //Primary key
        [Key]
        public int Crew_ID { get; set; }

        //Required foreign key foreman
        [Required(ErrorMessage = "Please select a foreman.")]        
        public int Crew_Foreman { get; set; }

        //Required foreign key member 1
        [Required(ErrorMessage = "Please select a member #1.")]
        public int Crew_Member_1 { get; set; }

        //Required foreign key member 2
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

        //Navigation Collection
        public ICollection<Service> Services { get; set; } = new List<Service>();  // skip navigation property for many-to-many relationship, see textbook


        //Helper Short
        public string CrewDisplayShort => $"{Foreman?.Emp_Last_Name}";

        //Helper Long
        public string CrewDisplayLong => $"{Foreman?.Emp_Last_Name}, {CrewMember1?.Emp_Last_Name}, {CrewMember2?.Emp_Last_Name}";

        // Ensures all crew positions are filled by different employees.
        //A ValidationResult if duplicate employee IDs are found; otherwise nothing.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Collect all assigned employee IDs into a single array for comparison
            var ids = new[] { Crew_Foreman, Crew_Member_1, Crew_Member_2 };

            // If duplicates exist, the distinct count will be less than 3
            if (ids.Distinct().Count() != 3)
            {
                yield return new ValidationResult(
                    "Crew must contain three distinct employees."
                );
            }
        }
    }
}
