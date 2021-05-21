using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagerApp.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EmployeeContext context;

        public UnitOfWork(EmployeeContext context)
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
           return await context.SaveChangesAsync();
        }
    }
}
