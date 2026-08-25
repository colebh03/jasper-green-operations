/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      CrewFilterViewModel.cs (for Service)

PURPOSE:      ViewModel used to support filtering service records by Crew.
              It supplies both the selected Crew_ID and the list of available
              Crews for user selection in the UI.

INPUT:        User selection of a Crew_ID from a dropdown list  

PROCESS:      - Validates that a Crew is selected
              - Passes selected Crew_ID back to controller for filtering
              - Provides list of Crews for dropdown population

OUTPUT:       - Supplies data to Razor View for rendering filter form
              - Returns selected Crew_ID to controller upon submission

HONOR CODE:   On my honor, as an Aggie, I have neither given nor received
              unauthorized aid on this academic work.
============================================================================
*/

using System.ComponentModel.DataAnnotations;

namespace JasperGreen.Models
{
    public class CrewFilterViewModel
    {
        // Holds the Crew selected by the user from the dropdown filter
        // Nullable so the form can load without a pre-selected value
        // Required enforces that the user must choose a Crew before submitting
        [Required(ErrorMessage = "Please select a crew.")]
        public int? Crew_ID { get; set; }

        // Collection of all Crews used to populate the dropdown list in the view
        // Initialized to avoid null reference issues if the controller fails to populate it
        public List<Crew> Crews { get; set; } = new();
    }
}

