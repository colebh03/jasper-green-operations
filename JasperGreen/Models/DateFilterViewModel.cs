/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      DateFilterViewModel.cs

PURPOSE:      ViewModel used to support filtering records by a date range.
              It supplies both a StartDate and EndDate for use in filtering
              operations throughout the application.

INPUT:        User-selected StartDate and EndDate values from date picker inputs

PROCESS:      - Validates that both dates are entered
              - Passes selected date range back to controller for filtering
              - Ensures date values are available for Razor form binding

OUTPUT:       - Supplies data to Razor View for rendering date filter form
              - Returns selected date range to controller upon submission

HONOR CODE:   On my honor, as an Aggie, I have neither given nor received
              unauthorized aid on this academic work.
============================================================================
*/

using System.ComponentModel.DataAnnotations;

namespace JasperGreen.Models
{
    public class DateFilterViewModel
    {
        // Holds the starting date selected by the user
        // Nullable so the form can initially load empty
        [Required(ErrorMessage = "Please select a start date.")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        // Holds the ending date selected by the user
        // Nullable so the form can initially load empty
        [Required(ErrorMessage = "Please select an end date.")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
    }
}