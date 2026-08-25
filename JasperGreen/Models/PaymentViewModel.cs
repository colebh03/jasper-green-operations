/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      PaymentViewModel.cs

PURPOSE:      ViewModel used to support creating and editing Payment records.
              It supplies the Payment entity along with supporting data such
              as the list of Customers and the current form mode (Add or Edit).

INPUT:        - User input for Payment fields from form submission
              - Selection of a Customer from a dropdown list

PROCESS:      - Combines Payment entity with Customer list for UI rendering
              - Tracks whether the form is in Add or Edit mode
              - Passes user-entered data back to controller for processing

OUTPUT:       - Supplies data to Razor View for rendering Add/Edit form
              - Returns populated Payment object to controller upon submission

HONOR CODE:   On my honor, as an Aggie, I have neither given nor received
              unauthorized aid on this academic work.
============================================================================
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace JasperGreen.Models
{
    // Property entity representing an individual payment in the system
    public class PaymentViewModel
    {
        // List of all customers used to populate the Customer dropdown
        public List<Customer> Customers { get; set; } = new List<Customer>();

        // The Payment being added or edited
        public Payment Payment { get; set; } = new Payment();

        // Indicates whether the form is in Add or Edit mode
        public string Action { get; set; } = string.Empty;

    }
}

