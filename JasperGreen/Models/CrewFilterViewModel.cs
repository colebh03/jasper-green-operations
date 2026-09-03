using System.ComponentModel.DataAnnotations;

namespace JasperGreen.Models
{
    public class CrewFilterViewModel
    {        
        [Required(ErrorMessage = "Please select a crew.")]

        public int? Crew_ID { get; set; }

        // Available crews used to populate the dropdown filter
        public List<Crew> Crews { get; set; } = new();
    }
}

