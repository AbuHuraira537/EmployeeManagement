using EmployeeManagerApp.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagerApp.ViewModels
{
    public class ViewUsersAndRolesViewModel
    {
        public ViewUsersAndRolesViewModel()
        {
            AllRoles = new List<IdentityRole>();
            AllUsers = new List<Users>();
        }
       public List<Users> AllUsers { get; set; }
       public List<IdentityRole> AllRoles { get; set; }
    }
}
