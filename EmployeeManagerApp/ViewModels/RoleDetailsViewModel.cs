using EmployeeManagerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagerApp.ViewModels
{
    public class RoleDetailsViewModel
    {
       public List<Users> InRole { get; set; }
        public List<Users> NotInRole { get; set; }
      

    }
}
