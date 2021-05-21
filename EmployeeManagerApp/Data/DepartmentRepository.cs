using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmployeeManagerApp.Data
{
    public class DepartmentRepository :  IDepartmentRepository
    {
        private readonly EmployeeContext context;

        public DepartmentRepository(EmployeeContext context)
        {
            this.context = context;
        }

        public async Task Add(Department entity)
        {
            entity.CreatedTime = DateTime.Now;
            entity.UpdatedTime = DateTime.Now;
           await context.Set<Department>().AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<Department> entities)
        {
            await context.Set<Department>().AddRangeAsync(entities);
        }

        public async Task<IEnumerable<Department>> Find(Expression<Func<Department, bool>> predicate)
        {
          return await context.Set<Department>().Where(predicate).ToListAsync();
        }

        public async Task<Department> Get(int id)
        {
            return await context.Set<Department>().Include(dept=>dept.Employees).FirstOrDefaultAsync(dept=>dept.Id==id);
        }

        public async Task<IEnumerable<Department>> GetAll()
        {
            return await context.Set<Department>().Include(dept => dept.Employees).ToListAsync();
        }

        public void Remove(Department entity)
        {
            context.Set<Department>().Include(dept=>dept.Employees).FirstOrDefaultAsync(dept=>dept.Id==entity.Id);
        }
        public async Task Remove(int id)
        {
           Department department = await context.Set<Department>().Include(dept => dept.Employees).FirstOrDefaultAsync(dept => dept.Id ==id);
            context.RemoveRange(department.Employees);
            context.Remove(department);
        }
        public void RemoveRange(IEnumerable<Department> entities)
        {
            context.Set<Department>().RemoveRange(entities);
        }

        public async Task<IEnumerable<Department>> SearchDepartment(string query)
        {
            return await context.Set<Department>().Include(emp => emp.Employees).Where(dept => dept.DepartmentName.Contains(query)).ToListAsync();
            
        }
        public async Task<Department> UpdateDepartment(Department department)
        {

            department.UpdatedTime = DateTime.Now;
            context.Entry(department).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return department;
        }
    }
}
