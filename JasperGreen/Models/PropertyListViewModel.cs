using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace JasperGreen.Models
{
    public class PropertyListViewModel
    {
        public List<Property> Properties { get; set; }

        public string SortColumn { get; set; }

        public string SortDirection { get; set; }

        public string Filter { get; set; }

        public int? Id { get; set; }

        public string CurrentFilterText { get; set; }
    }
}