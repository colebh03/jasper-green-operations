/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      PropertyFilterViewModel.cs (for Service)

PURPOSE:      ViewModel used to support filtering service records by Property.
              It supplies both the selected Property_ID and the list of available
              Properties for user selection in the UI.

INPUT:        User selection of a Property_ID from a dropdown list  

PROCESS:      - Validates that a Property is selected
              - Passes selected Property_ID back to controller for filtering
              - Provides list of Properties for dropdown population

OUTPUT:       - Supplies data to Razor View for rendering filter form
              - Returns selected Property_ID to controller upon submission

HONOR CODE:   On my honor, as an Aggie, I have neither given nor received
              unauthorized aid on this academic work.
============================================================================
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace JasperGreen.Models
{
    public class PropertyFilterViewModel
    {
        // Holds the Property selected by the user from the dropdown filter
        // Nullable so the form can load without a pre-selected value
        // Required enforces that the user must choose a Property before submitting
        [Required(ErrorMessage = "Please select a property.")]
        public int? Property_ID { get; set; }

        // Collection of all Properties used to populate the dropdown list in the view
        // Initialized to avoid null reference issues if the controller fails to populate it
        public List<Property> Properties { get; set; } = new();
    }
}

