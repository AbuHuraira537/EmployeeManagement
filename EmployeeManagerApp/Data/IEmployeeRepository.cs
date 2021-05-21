using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmployeeManagerApp.Data
{
    public interface IEmployeeRepository 
    {
        Task Add(Employee entity, int EmployeeDept);
        Task AddRange(IEnumerable<Employee> entities);
        //Getting
        Task<Employee> Get(int id);
        Task<IEnumerable<Employee>> GetAll();
        Task<IEnumerable<Employee>> Find(Expression<Func<Employee, bool>> predicate);

        //Removing
        void Remove(Employee entity);
        void RemoveRange(IEnumerable<Employee> entities);
        //bool order, if true then ascending order, flase descending order
        Task<IEnumerable<Employee>> GetSortedEmployeesByColumn(string column,bool order,int pageNumber,int pageSize);
        //type means from the whole company or by departments
        Task<IEnumerable<Employee>> GetEmployeesBySalary(int range,bool type);
        Task<IEnumerable<Employee>> SearchByName(string name);
        Task<IEnumerable<Employee>> CommonSearch(string query);
        Task<IEnumerable<Employee>> GetEmployeesPaged(int pageNumber, int pageSize);
        Task<IEnumerable<SelectListItem>> SelectListItemsOfEmployee();
        Employee UpdateEmployee(Employee employee);
        Task DeleteEmployee(int id);
        int GetTotalCountOFEmployees();
        
    }
}
