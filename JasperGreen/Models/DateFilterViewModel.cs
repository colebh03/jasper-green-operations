using System.ComponentModel.DataAnnotations;

namespace JasperGreen.Models
{
    public class DateFilterViewModel
    {        
        [Required(ErrorMessage = "Please select a start date.")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        
        [Required(ErrorMessage = "Please select an end date.")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
    }
}