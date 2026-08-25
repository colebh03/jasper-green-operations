using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema; // for NotMapped

namespace JasperGreen.Models
{
    public class User : IdentityUser
    {
        // Inherits all IdentityUser properties
        [NotMapped] 
        public IList<string> RoleNames { get; set; } = null!;
    }
    
}
