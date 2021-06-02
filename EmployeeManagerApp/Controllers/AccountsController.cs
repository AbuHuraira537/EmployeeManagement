using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.ViewModels;
using EmployeeManagerApp.Authentication;
using EmployeeManagerApp.Models;
using EmployeeManagerApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagerApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AccountsController : Controller
    {
        private readonly IAccountManager accountManager;

        public AccountsController(IAccountManager accountManager)
        {
            this.accountManager = accountManager;
        }
        public async Task<IActionResult> Index()
        {
            ViewUsersAndRolesViewModel model = new ViewUsersAndRolesViewModel
            {
                AllUsers = await accountManager.GetAll(),
                AllRoles = await accountManager.GetAllRoles()

        };
            
           //var users = await accountManager.GetAll();
            return View(model);
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(CreateAccountViewModel user)
        {
            //get error response in res 
          var res = await accountManager.CreateAccountAsync(user);
            if (res == null)
            {
                ViewUsersAndRolesViewModel model = new ViewUsersAndRolesViewModel
                {
                    AllUsers = await accountManager.GetAll(),
                    AllRoles = await accountManager.GetAllRoles()

                };
                return View(nameof(Index), model);
            }
            foreach(var err in res)
            {
                ModelState.AddModelError("", err.Description.ToString());
            }
            return View(user);
        }
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel role)
        {
          var res= await accountManager.CreateRole(role);
            if (res == null)
            {

                ViewUsersAndRolesViewModel model = new ViewUsersAndRolesViewModel
                {
                    AllUsers = await accountManager.GetAll(),
                    AllRoles = await accountManager.GetAllRoles()

                };
                return View(nameof(Index), model);
            }
            foreach (var err in res)
            {
                ModelState.AddModelError("", err.Description.ToString());
            }
            return View(role);
        }
       public async Task<IActionResult> Details(string id)
        {
            return View(await accountManager.Get(id));
        }

        public async Task<IActionResult> RoleDetails(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await accountManager.GetRole(roleId);
            var usersWithRole =await accountManager.GetUsersWithRole(role.Name);
           var usersNotInRole =await accountManager.GetUsersNotInRole(role.Name);
            RoleDetailsViewModel model = new RoleDetailsViewModel
            {
                InRole = usersWithRole,
                NotInRole = usersNotInRole,
        };
            return View(model);

        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string roleId)
        {
            ViewBag.roleId = roleId;
            List<UserRoleViewModel> model = new List<UserRoleViewModel>();
            var role = await accountManager.GetRole(roleId);
            var usersWithRole = await accountManager.GetUsersWithRole(role.Name);
            var usersNotInRole = await accountManager.GetUsersNotInRole(role.Name);
            RoleDetailsViewModel temp = new RoleDetailsViewModel
            {
                InRole = usersWithRole,
                NotInRole = usersNotInRole,
            };
            foreach(var user in temp.InRole)
            {
                model.Add(new UserRoleViewModel
                {
                    IsSelected = true,
                    UserId = user.Id,
                    UserName = user.UserName
                });
            }
            foreach (var user in temp.NotInRole)
            {
                model.Add(new UserRoleViewModel
                {
                    IsSelected = false,
                    UserId = user.Id,
                    UserName = user.UserName
                });
            }

            return View(model);
        }
        public async Task<IActionResult> UpdateRoleUsers(List<UserRoleViewModel> model,string roleId)
        {
            var role = await accountManager.GetRole(roleId);
            for(int i = 0; i < model.Count(); i++)
            {
                if (model[i].IsSelected)
                {
                   await accountManager.AddUserInRole(await accountManager.Get(model[i].UserId),role.Name);
                }
                else
                {
                    await accountManager.RemoveUserFromRole(await accountManager.Get(model[i].UserId), role.Name);
                }
            }
            return View("Login");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            bool res = await accountManager.Login(model);
            if(res)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Invalid credetioals");
            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await accountManager.Logout();
            return RedirectToAction(nameof(Login));
        }
        

    }
}