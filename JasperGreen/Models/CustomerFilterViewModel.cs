/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      CustomerFilterViewModel.cs

PURPOSE:      ViewModel used to support filtering service records by Customer.
              It supplies both the selected Cust_ID and the list of available
              Customers for user selection in the UI.

INPUT:        User selection of a Cust_ID from a dropdown list  

PROCESS:      - Validates that a Customer is selected
              - Passes selected Cust_ID back to controller for filtering
              - Provides list of Customers for dropdown population

OUTPUT:       - Supplies data to Razor View for rendering filter form
              - Returns selected Cust_ID to controller upon submission

HONOR CODE:   On my honor, as an Aggie, I have neither given nor received
              unauthorized aid on this academic work.
============================================================================
*/

using System.ComponentModel.DataAnnotations;

namespace JasperGreen.Models
{
    public class CustomerFilterViewModel
    {
        // Holds the Customer selected by the user from the dropdown filter
        // Nullable so the form can load without a pre-selected value
        // Required enforces that the user must choose a Customer before submitting
        [Required(ErrorMessage = "Please select a customer.")]
        public int? Cust_ID { get; set; }

        // Collection of all Customers used to populate the dropdown list in the view
        // Initialized to avoid null reference issues if the controller fails to populate it
        public List<Customer> Customers { get; set; } = new();
    }
}