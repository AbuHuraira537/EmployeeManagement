using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmployeeManagerApp.Data
{
    public interface IDepartmentRepository 
    {
        Task Add(Department entity);
        Task AddRange(IEnumerable<Department> entities);
        //Getting
        Task<Department> Get(int id);
        Task<IEnumerable<Department>> GetAll();
        IEnumerable<Department> GetAllDepartmentsSync();
        Task<IEnumerable<Department>> Find(Expression<Func<Department, bool>> predicate);
        Task<Department> UpdateDepartment(Department department);

        //Removing
        void Remove(Department entity);
        Task Remove(int id);
        void RemoveRange(IEnumerable<Department> entities);
        Task<IEnumerable<Department>> SearchDepartment(string query);
    }
}
