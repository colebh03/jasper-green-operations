using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace JasperGreen.Models
{
    public class CustomerListViewModel
    {
        public List<Customer> Customers { get; set; }

        public string SortColumn { get; set; }

        public string SortDirection { get; set; }

        public string Filter { get; set; }

        public int? Id { get; set; }

        public string CurrentFilterText { get; set; }
    }
}