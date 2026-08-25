/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      ServiceViewModel.cs

PURPOSE:      ViewModel used to support creating and editing service records.
              It supplies the Service entity along with supporting data
              such as Customers, Crews, and Properties for dropdown selection.

INPUT:        - User input for service fields from form submission
              - Selection of Customer, Crew, and Property from dropdown lists

PROCESS:      - Combines Service entity with related lists for UI rendering
              - Tracks whether the form is in Add or Edit mode
              - Passes user-entered data back to controller for processing

OUTPUT:       - Supplies data to Razor View for rendering Add/Edit form
              - Returns populated Service object to controller upon submission

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
    // Property entity representing an individual service in the system
    public class ServiceViewModel
    {
        // List of all customers used to populate the Customer dropdown
        public List<Customer> Customers { get; set; } = new List<Customer>();

        // List of all crews used to populate the Crew dropdown
        public List<Crew> Crews { get; set; } = new List<Crew>();

        // List of all properties used to populate the Property dropdown
        public List<Property> Properties { get; set; } = new List<Property>();

        // The service record being added or edited
        public Service Service { get; set; } = new Service();

        // Indicates whether the form is in Add or Edit mode
        public string Action { get; set; } = string.Empty;

    }
}