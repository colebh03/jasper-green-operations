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
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter a username.")]
        [StringLength(255)]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter a password.")]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty; public string ReturnUrl { get; set; } = string.Empty; public bool RememberMe { get; set; }
    }
}

