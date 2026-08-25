/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      PropertyViewModel.cs

PURPOSE:      Serves as a ViewModel to encapsulate Property data along with
              related UI data (Customers and States) for Add/Edit views.

INPUT:        Property data and related collections retrieved from the
              database and helper utilities.

PROCESS:      Combines the Property entity with supporting collections
              required for dropdowns and form state management.

OUTPUT:       A structured object passed from controller to view for
              rendering forms with all required data.

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
    // Property entity representing an individual property in the system
    public class PropertyViewModel
    {
        // List of all customers used to populate the Customer dropdown
        public List<Customer> Customers { get; set; } = new List<Customer>();

        // The Incident being added or edited
        public Property Property { get; set; } = new Property();

        // Indicates whether the form is in Add or Edit mode
        public string Action { get; set; } = string.Empty;

        // For populating state dropdown box
        public List<SelectListItem> States { get; set; } = new();
    }
}

