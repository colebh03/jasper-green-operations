/*
============================================================================
AUTHOR:       Cole Howell, Michael Hudgins
COURSE:       ISTM 415
PROGRAM:      CrewViewModel.cs

PURPOSE:      Serves as a ViewModel to encapsulate Crew data along with
              related Employee data required for dropdown selections in views.

INPUT:        Data retrieved from the database including Crew entities and
              associated Employee records.


PROCESS:      Combines Crew and Employee data into a single object to support
              strongly-typed view rendering and form binding.

OUTPUT:       A structured ViewModel used by Razor views for creating and
              editing Crew records.

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
    // Crew entity representing an individual crew in the system
    public class CrewViewModel
    {
        // List of employees for ALL dropdowns
        public List<Employee> Employees { get; set; } = new List<Employee>();

        /// The crew being added or edited
        public Crew Crew { get; set; } = new Crew();

        // Indicates whether the form is in Add or Edit mode
        public string Action { get; set; } = string.Empty;
    }
}

