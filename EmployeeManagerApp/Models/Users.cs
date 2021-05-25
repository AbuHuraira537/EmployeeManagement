using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagerApp.Models
{
    public class Users : IdentityUser
    {
        [Required]
        [Display(Name ="Display Name")]
        public string DisplayName { get; set; }
        public string Address { get; set; }
    }
}
