using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace JasperGreen.Models
{
    public class UserViewModel
    {
        public IEnumerable<User> Users { get; set; } = new List<User>();
    }
}
