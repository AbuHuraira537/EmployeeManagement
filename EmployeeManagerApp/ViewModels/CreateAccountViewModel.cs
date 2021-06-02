using EmployeeManagerApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class CreateAccountViewModel
    {
        public CreateAccountViewModel()
        {
            Account = new Users();
        }
        public Users Account { get; set; }
        [Required]
     
        public string Password { get; set; }
        [Compare("Password",ErrorMessage ="password and confirm password must same")]
        [Display(Name ="Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
