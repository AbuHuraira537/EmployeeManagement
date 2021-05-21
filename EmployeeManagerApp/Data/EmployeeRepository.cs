using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmployeeManagerApp.Data
{
    public class EmployeeRepository :  IEmployeeRepository
    {
        private readonly EmployeeContext context;

        public EmployeeRepository(EmployeeContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Employee>> GetEmployeesPaged(int pageNumber,int pageSize)
        {
           return await context.Set<Employee>().Include(emp => emp.EmployeeDept).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();

        }
        public async Task<IEnumerable<Employee>> CommonSearch(string query)
        {
            List<Employee> employees;
            int salary;
            if (Int32.TryParse(query, out salary))
            {
                employees = await context.Set<Employee>().Include(emp => emp.EmployeeDept).Where(emp => emp.Salary <= salary + 10000 && emp.Salary >= salary - 10000).ToListAsync();
                return employees;
            }
            employees = await context.Set<Employee>().Include(emp => emp.EmployeeDept).Where(emp => emp.EmployeeName.Contains(query) || emp.EmployeeDept.DepartmentName.Contains(query)).ToListAsync();
            return employees;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesBySalary(int range, bool type)
        {

            if (type)
            {
                List<Department> departments = await context.Set<Department>().Include(dept => dept.Employees).ToListAsync();
                List<Employee> empls = new List<Employee>();
                if (range == 1)
                {
                    foreach (Department dept in departments)
                    {
                        int? maxSal = dept.Employees.Max(emp => emp.Salary);
                        List<Employee> temp = dept.Employees.Where(emp => emp.Salary == maxSal).ToList();
                        foreach (Employee em in temp)
                        {
                            empls.Add(em);
                        }
                    }
                    return empls;
                }
                else if (range == 0)
                {
                    foreach (Department dept in departments)
                    {
                        int? maxSal = dept.Employees.Min(emp => emp.Salary);
                        List<Employee> temp = dept.Employees.Where(emp => emp.Salary == maxSal).ToList();
                        foreach (Employee em in temp)
                        {
                            empls.Add(em);
                        }
                    }
                    return empls;
                }
                else if (range >= 90000)
                {
                    foreach (Department dept in departments)
                    {
                        List<Employee> temp = dept.Employees.Where(emp => emp.Salary > 90000).ToList();
                        foreach (Employee em in temp)
                        {
                            empls.Add(em);
                        }
                    }
                    return empls;
                }
                else
                {
                    foreach (Department dept in departments)
                    {
                        List<Employee> temp = dept.Employees.Where(emp => emp.Salary >= range && emp.Salary <= range + 20000).ToList();
                        foreach (Employee em in temp)
                        {
                            empls.Add(em);
                        }
                    }
                    return empls;
                }

            }
// count = await employeeContext.Set<Employee>().Include(emp => emp.EmployeeDept).Where(emp => emp.Salary > 90000).CountAsync();

            List<Employee> employees = await context.Set<Employee>().Include(emp => emp.EmployeeDept).ToListAsync();
            if (range >= 90000)
            {

                employees = employees.Where(emp => emp.Salary > 90000).ToList();

            }
            else if (range == 0)
            {
                int? minSalary = employees.Min(e => e.Salary);
                employees = employees.Where(emp => emp.Salary == minSalary).ToList();

            }
            else if (range == 1)
            {
                int? maxSalary = employees.Max(e => e.Salary);
                employees = employees.Where(emp => emp.Salary == maxSalary).ToList();

            }
            else
            {
                employees = await context.Set<Employee>().Include(emp => emp.EmployeeDept).Where(emp => emp.Salary >= range && emp.Salary <= range + 20000).ToListAsync();

            }
            return employees;
        }

        public async Task<IEnumerable<Employee>> GetSortedEmployeesByColumn(string column, bool order, int pageNumber, int pageSize)
        {
            
            List<Employee> employees = await context.Set<Employee>().Include(emp => emp.EmployeeDept).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
            switch (column)
            {
                case "EmployeeName":
                    {
                        if (order)
                        {
                            employees.Sort((a, b) => String.Compare(a.EmployeeName, b.EmployeeName));

                            //IEnumerable<Employee> lst = from employee in employees
                            //                     orderby employee.EmployeeName ascending
                            //                     select employee;
                            return  employees;
                        }
                        else
                        {
                            IEnumerable<Employee> lst = from employee in employees
                                                        orderby employee.EmployeeName descending
                                                        select employee;
                            return lst.ToList();
                        }

                    }
                case "DepartmentName":
                    {
                        if (order)
                        {
                            IEnumerable<Employee> lst = from employee in employees
                                                        orderby employee.EmployeeDept.DepartmentName ascending
                                                        select employee;
                            return  lst.ToList();
                        }
                        else
                        {
                            IEnumerable<Employee> lst = from employee in employees
                                                        orderby employee.EmployeeDept.DepartmentName descending
                                                        select employee;
                            return  lst.ToList();
                        }
                    }
                case "Salary":
                    {
                        if (order)
                        {
                            IEnumerable<Employee> lst = from employee in employees
                                                        orderby employee.Salary ascending
                                                        select employee;
                            return  lst.ToList();
                        }
                        else
                        {
                            IEnumerable<Employee> lst = from employee in employees
                                                        orderby employee.Salary descending
                                                        select employee;
                            return  lst.ToList();
                        }
                    }
                case "CreatedTime":
                    {
                        if (order)
                        {

                            employees.Sort((a, b) => DateTime.Compare(a.CreatedTime, b.CreatedTime));
                            //IEnumerable<Employee> lst = from employee in employees
                            //                            orderby employee.CreatedTime ascending
                            //                            select employee;
                            return  employees;
                        }
                        else
                        {
                            IEnumerable<Employee> lst = from employee in employees
                                                        orderby employee.CreatedTime descending
                                                        select employee;
                            return lst.ToList();
                        }
                    }
                case "UpdatedTime":
                    {
                        if (order)
                        {
                            IEnumerable<Employee> lst = from employee in employees
                                                        orderby employee.UpdatedTime ascending
                                                        select employee;
                            return  lst.ToList();
                        }
                        else
                        {
                            IEnumerable<Employee> lst = from employee in employees
                                                        orderby employee.UpdatedTime descending
                                                        select employee;
                            return  lst.ToList();
                        }
                    }
            }
            return null;
        }

        public async Task<IEnumerable<Employee>> SearchByName(string name)
        {
            List<Employee> employees;

            employees = await context.Set<Employee>().Include(emp => emp.EmployeeDept).Where(emp => emp.EmployeeName.Contains(name)).ToListAsync();

            return employees;
        }

        public async Task Add(Employee employee,int EmployeeDept)
        {
            employee.CreatedTime = DateTime.Now;
            employee.UpdatedTime = DateTime.Now;

            Department department = await context.Set<Department>().FindAsync(EmployeeDept);
            department.Employees.Add(employee);
            context.Set<Department>().Update(department);
            
        }

        public async Task AddRange(IEnumerable<Employee> entities)
        {
         await context.Set<Employee>().AddRangeAsync(entities);
        }

        public async Task<Employee> Get(int id)
        {
           return await context.Set<Employee>().Include(emp => emp.EmployeeDept).FirstOrDefaultAsync(emp => emp.Id == id);
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await context.Set<Employee>().Include(emp => emp.EmployeeDept).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> Find(Expression<Func<Employee, bool>> predicate)
        {
            return await context.Set<Employee>().Where(predicate).ToListAsync();
        }

        public void Remove(Employee entity)
        {
            context.Set<Employee>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<Employee> entities)
        {
            context.Set<Employee>().RemoveRange(entities);
        }
       public async Task<IEnumerable<SelectListItem>> SelectListItemsOfEmployee()
        {
            return await context.Set<Department>().Select(depts => new SelectListItem() { Text = depts.DepartmentName, Value = depts.Id.ToString() }).ToListAsync();
        }
        public  Employee UpdateEmployee(Employee employee)
        {
            employee.UpdatedTime = DateTime.Now;
            context.Entry(employee).State = EntityState.Modified;
            return employee;
        }
        public async Task DeleteEmployee(int id)
        {
            Employee employee = await context.Set<Employee>().FindAsync(id);
            context.Set<Employee>().Remove(employee);
        }
        public int GetTotalCountOFEmployees()
        {
           return context.Set<Employee>().Count();
        }
    }
}
