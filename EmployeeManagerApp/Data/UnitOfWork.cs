using EmployeeManagement.Models;
using EmployeeManagerApp.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagerApp.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EmployeeContext context;
  

        public UnitOfWork(EmployeeContext context, UserManager<Users> userManager)
        {
            this.context = context;
           
            Employees = new EmployeeRepository(context);
            Departments = new DepartmentRepository(context);
        }
        public IEmployeeRepository Employees { get; private set; }

        public IDepartmentRepository Departments { get; private set; }
      
       
        public void Dispose()
        {
            context.Dispose();
        }

        public async Task<int> Save()
        {
            try
            {
                return await context.SaveChangesAsync();
            }catch(Exception ex)
            {
                return 1;
            }
           
        }
    }
}
