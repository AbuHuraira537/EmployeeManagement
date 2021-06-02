using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using EmployeeManagerApp.Models;
using EmployeeManagerApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagerApp.Authentication
{
    public class AccountManager : IAccountManager
    {
        private readonly UserManager<Users> userManager;
        private readonly SignInManager<Users> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly EmployeeContext context;

        public AccountManager(UserManager<Users> userManager,SignInManager<Users> signInManager,RoleManager<IdentityRole> roleManager,EmployeeContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.context = context;
        }
        public async Task<List<IdentityError>> CreateAccountAsync(CreateAccountViewModel user)
        {
          var result =  await userManager.CreateAsync(user.Account, user.Password);
            if (result.Succeeded)
            {
                return null;
            }
            return result.Errors.ToList();
        }

        public async Task<List<IdentityError>> CreateRole(CreateRoleViewModel role)
        {
            IdentityRole r = new IdentityRole
            {
                Name = role.RoleName,
        };
            var result = await roleManager.CreateAsync(r);
            if (result.Succeeded)
            {
                return null;
            }
            return result.Errors.ToList();
        }

        public async Task<Users> Get(string Id)
        {
           return await userManager.Users.Where(u=>u.Id==Id).FirstOrDefaultAsync();
        }

        public async Task<List<Users>> GetAll()
        {
            return await userManager.Users.ToListAsync();
        }
        public async Task<List<Users>> GetUsersWithRole(string role)
        {
            List<Users> users = new List<Users>();
            List<Users> temp = new List<Users>();
            users = await  userManager.Users.ToListAsync();
            for(int i=0;i< users.ToList().Count();i++)
            {
                if(await userManager.IsInRoleAsync(users[i],role))
                {
                    temp.Add(users[i]);
                }
            }
            return temp;
        }
        public async Task<List<Users>> GetUsersNotInRole(string role)
        {
           

            List<Users> users = new List<Users>();
            List<Users> temp = new List<Users>();
            users = await userManager.Users.ToListAsync();
            for (int i = 0; i < users.Count(); i++)
            {
                if (!await userManager.IsInRoleAsync(users[i], role))
                {
                    temp.Add(users[i]); 
                }
            }
            return temp;
        }
        public async Task<IdentityResult> AddUserInRole(Users user, string role)
        {
            var res = await userManager.AddToRoleAsync(user, role);
            return res;
        }
        public async Task<IdentityResult> RemoveUserFromRole(Users user, string role)
        {
            var res = await userManager.RemoveFromRoleAsync(user, role);
            return res;
        }
        public async Task<List<IdentityRole>> GetAllRoles()
        {
          return await roleManager.Roles.ToListAsync();
            
        }

        public async Task<IdentityRole> GetRole(string id)
        {
         return  await roleManager.FindByIdAsync(id);
        }

        public async Task<bool> Login(LoginViewModel login)
        {
            SignInResult result;
            try
            {
                 result = await signInManager.PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, false);

            }catch(Exception ex)
            {
                return false;
            }
            if (result.Succeeded)
            {
                return result.Succeeded;
            }
         return  false;
        }

        public async Task Logout()
        {
           await signInManager.SignOutAsync();
        }
    }
}
