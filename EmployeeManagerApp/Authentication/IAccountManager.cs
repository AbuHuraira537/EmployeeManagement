using EmployeeManagement.ViewModels;
using EmployeeManagerApp.Models;
using EmployeeManagerApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagerApp.Authentication
{
    public interface IAccountManager
    {
        Task<List<IdentityError>> CreateAccountAsync(CreateAccountViewModel user);
        Task<List<IdentityError>> CreateRole(CreateRoleViewModel role);
        Task<List<Users>> GetAll();
        Task<List<IdentityRole>> GetAllRoles();
        Task<IdentityRole> GetRole(string id);
        Task<List<Users>> GetUsersWithRole(string role);
        Task<List<Users>> GetUsersNotInRole(string role);
        Task<IdentityResult> AddUserInRole(Users user, string role);
        Task<IdentityResult> RemoveUserFromRole(Users user,string role);
        Task<Users> Get(string Id);
        Task<bool> Login(LoginViewModel login);
        Task Logout();
       
    }
}
