using EmployeeManagerApp.Models;
using EmployeeManagerApp.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagerApp.CustomeMiddleWares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class BlazorCookieLoginMiddleware
    {
        public static IDictionary<Guid, LoginViewModel> Logins { get; private set; }
           = new ConcurrentDictionary<Guid, LoginViewModel>();

        private readonly RequestDelegate _next;

        public BlazorCookieLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, SignInManager<Users> signInMgr)
        {
            if (context.Request.Path == "/login" && context.Request.Query.ContainsKey("key"))
            {
                var key = Guid.Parse(context.Request.Query["key"]);
                var info = Logins[key];

                var result = await signInMgr.PasswordSignInAsync(info.UserName, info.Password, false, lockoutOnFailure: true);
                info.Password = null;
                if (result.Succeeded)
                {
                    Logins.Remove(key);
                    context.Response.Redirect("/");
                    return;
                }
                else if (result.RequiresTwoFactor)
                {
                    //TODO: redirect to 2FA razor component
                    context.Response.Redirect("/loginwith2fa/" + key);
                    return;
                }
                else
                {
                    //TODO: Proper error handling
                    context.Response.Redirect("/loginfailed");
                    return;
                }
            }
            else
            {
                await _next.Invoke(context);
            }
         

            
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class BlazorCookieLoginMiddlewareExtensions
    {
        public static IApplicationBuilder UseBlazorCookieLoginMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BlazorCookieLoginMiddleware>();
        }
    }
}
