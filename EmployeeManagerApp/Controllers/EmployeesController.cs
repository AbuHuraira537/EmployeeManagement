using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagerApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagerApp.Controllers
{
    [Authorize(Roles ="Employee")]
    public class EmployeesController : Controller
    {
        private readonly EmployeeContext employeeContext;
        private readonly IUnitOfWork unitOfWork;
        public static int count=0;
        private const int pageSize = 10;
        public EmployeesController(EmployeeContext employeeContext,IUnitOfWork unitOfWork)
        {
            this.employeeContext = employeeContext;
            this.unitOfWork = unitOfWork;
        }
        // GET: Default
        public async Task<ActionResult> Index(int pageNumber)
        {
            // count = employeeContext.Set<Employee>().Count();

            //List<Employee> employees = await employeeContext.Set<Employee>().Include(emp=>emp.EmployeeDept).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
            // List<Employee> employees = await employeeContext.Set<Employee>().Include(emp => emp.EmployeeDept).ToListAsync();
            //return View(employees);

            ViewBag.current = pageNumber;
            count = unitOfWork.Employees.GetTotalCountOFEmployees();
            ViewBag.totalPages = count / pageSize;
            return View(await unitOfWork.Employees.GetEmployeesPaged(pageNumber, pageSize));
        }

        // GET: Default/Details/5
        public async Task<ActionResult> Details(int id)
        {
          return View(await unitOfWork.Employees.Get(id));
           // return View(await employeeContext.Set<Employee>().Include(emp => emp.EmployeeDept).FirstOrDefaultAsync(emp => emp.Id == id));
        }

        // GET: Default/Create
        public async Task<ActionResult> Create()
        {
            //  ViewBag.departments = await employeeContext.Set<Department>().Select(depts => new SelectListItem() { Text = depts.DepartmentName, Value = depts.Id.ToString() }).ToListAsync();
            ViewBag.departments = await unitOfWork.Employees.SelectListItemsOfEmployee();
            return View();

        }

        // POST: Default/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Employee employee, int EmployeeDept)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //employee.CreatedTime = DateTime.Now;
                    //employee.UpdatedTime = DateTime.Now;
                    //Department department = await employeeContext.Set<Department>().FindAsync(EmployeeDept);
                    //department.Employees.Add(employee);
                    //employeeContext.Set<Department>().Update(department);
                    //await employeeContext.SaveChangesAsync();

                    await unitOfWork.Employees.Add(employee, EmployeeDept);
                    await unitOfWork.Save();
                    return RedirectToAction(nameof(Index));
                }
                // TODO: Add insert logic here
                ModelState.AddModelError("", "Please enter valid values and required values.");
                return View(employee);
            }
            catch
            {
                return View();
            }
        }

        // GET: Default/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            // ViewBag.departments = await employeeContext.Set<Department>().Select(depts => new SelectListItem() { Text = depts.DepartmentName, Value = depts.Id.ToString() }).ToListAsync();

            //    return View(await employeeContext.Set<Employee>().FirstOrDefaultAsync(emp => emp.Id == id));
            return View(await unitOfWork.Employees.Get(id));
        }

        // POST: Deparments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Employee employee)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    //employee.UpdatedTime = DateTime.Now;
                    //employeeContext.Entry(employee).State = EntityState.Modified;
                    //await employeeContext.SaveChangesAsync();
                    unitOfWork.Employees.UpdateEmployee(employee);
                   await unitOfWork.Save();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Please entervalid values");
                return View(employee);


            }
            catch
            {
                return View();
            }
        }
        public async Task<ActionResult> Search(string search)
        {


            //List<Employee> employees;
            //int salary;
            //if (Int32.TryParse(search, out salary))
            //{
            //    employees = await employeeContext.Set<Employee>().Include(emp => emp.EmployeeDept).Where(emp => emp.Salary <= salary + 10000 && emp.Salary >= salary - 10000).ToListAsync();
            //    return View(nameof(Index), employees);
            //}
            //employees = await employeeContext.Set<Employee>().Include(emp => emp.EmployeeDept).Where(emp => emp.EmployeeName.Contains(search) || emp.EmployeeDept.DepartmentName.Contains(search)).ToListAsync();
            IEnumerable<Employee> employees = await unitOfWork.Employees.CommonSearch(search);
            return View(nameof(Index), employees.ToList());
        }
        public async Task<ActionResult> EmployeeSearch(string search,int pageNumber,bool etype)
        {
            ViewBag.current = pageNumber;
            IEnumerable<Employee> employees;
            employees = await unitOfWork.Employees.SearchByName(search);
            count = employees.Count();
            ViewBag.totalPages = count / pageSize;
            return View(nameof(Index), employees.ToList());
            // count = employeeContext.Set<Employee>().Count();
            // employees = await employeeContext.Set<Employee>().Include(emp => emp.EmployeeDept).Where(emp => emp.EmployeeName.Contains(search)).ToListAsync();
            //paginatin purposes

        }
        public ActionResult DepartmentSearch(string search, int pageNumber, bool dtype)
        {
            return RedirectToAction("index", "Deparments", new { search = search });
        }
        public async Task<ActionResult> SalarySearch(int search, int pageNumber, bool stype)
        {

            // if (stype)
            // {
            //   List<Department> departments = await employeeContext.Set<Department>().Include(dept => dept.Employees).ToListAsync();
            //     List<Employee> empls=new List<Employee>();
            //     if (search == 1)
            //     {
            //         foreach (Department dept in departments)
            //         {
            //             int? maxSal = dept.Employees.Max(emp => emp.Salary);
            //             List<Employee> temp = dept.Employees.Where(emp => emp.Salary == maxSal).ToList();
            //             foreach (Employee em in temp)
            //             {
            //                 empls.Add(em);
            //             }
            //         }
            //         return View(nameof(Index), empls);
            //     }
            //     else if (search == 0)
            //     {
            //         foreach (Department dept in departments)
            //         {
            //             int? maxSal = dept.Employees.Min(emp => emp.Salary);
            //             List<Employee> temp = dept.Employees.Where(emp => emp.Salary == maxSal).ToList();
            //             foreach (Employee em in temp)
            //             {
            //                 empls.Add(em);
            //             }
            //         }
            //         return View(nameof(Index), empls);
            //     }
            //     else if (search >= 90000)
            //     {
            //         foreach (Department dept in departments)
            //         {
            //             List<Employee> temp = dept.Employees.Where(emp => emp.Salary >90000).ToList();
            //             foreach (Employee em in temp)
            //             {
            //                 empls.Add(em);
            //             }
            //         }
            //         return View(nameof(Index), empls);
            //     }
            //     else
            //     {
            //         foreach (Department dept in departments)
            //         {
            //             List<Employee> temp = dept.Employees.Where(emp => emp.Salary >= search && emp.Salary <= search + 20000).ToList();
            //             foreach (Employee em in temp)
            //             {
            //                 empls.Add(em);
            //             }
            //         }
            //         return View(nameof(Index), empls);
            //     }

            // }

            //// count = await employeeContext.Set<Employee>().Include(emp => emp.EmployeeDept).Where(emp => emp.Salary > 90000).CountAsync();

            // List<Employee> employees = await employeeContext.Set<Employee>().Include(emp => emp.EmployeeDept).ToListAsync();
            // if (search >= 90000)
            // {

            //     employees = employees.Where(emp => emp.Salary > 90000).ToList();

            // }
            // else if (search == 0)
            // {
            //  int? minSalary= employees.Min(e=>e.Salary);
            //    employees = employees.Where(emp => emp.Salary == minSalary).ToList();

            // }
            // else if (search == 1)
            // {
            //    int? maxSalary = employees.Max(e => e.Salary);
            //     employees = employees.Where(emp => emp.Salary == maxSalary).ToList();

            // }
            // else
            // {
            //     employees = await employeeContext.Set<Employee>().Include(emp => emp.EmployeeDept).Where(emp => emp.Salary >= search && emp.Salary <= search + 20000).ToListAsync();

            // }
            IEnumerable<Employee> employees = await unitOfWork.Employees.GetEmployeesBySalary(search, stype);
            ViewBag.current = pageNumber;
            count = employees.Count();
            ViewBag.totalPages = count / pageSize;
            return View(nameof(Index), employees.ToList());
        }

       




        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                //Employee employee = await employeeContext.Set<Employee>().FindAsync(id);
                //employeeContext.Set<Employee>().Remove(employee);
                //await employeeContext.SaveChangesAsync();
                await unitOfWork.Employees.DeleteEmployee(id);
                await unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]

        public async Task<ActionResult> Sorting(bool type, string field,int pageNumber)
        {
            ViewBag.totalPages = count/pageSize;
            ViewBag.current = pageNumber;
           IEnumerable<Employee> employees = await unitOfWork.Employees.GetSortedEmployeesByColumn(field, type, pageNumber, pageSize);
            return View(nameof(Index), employees.ToList());

            //List<Employee> employees = await employeeContext.Set<Employee>().Include(emp => emp.EmployeeDept).Skip(pageNumber*pageSize).Take(pageSize).ToListAsync();
            //switch (field)
            //{
            //    case "EmployeeName":
            //        {
            //            if (type == 1)
            //            {
            //                employees.Sort((a, b) => String.Compare(a.EmployeeName, b.EmployeeName));

            //                //IEnumerable<Employee> lst = from employee in employees
            //                //                     orderby employee.EmployeeName ascending
            //                //                     select employee;
            //                return View(nameof(Index), employees);
            //            }
            //            else
            //            {
            //                IEnumerable<Employee> lst = from employee in employees
            //                                            orderby employee.EmployeeName descending
            //                                            select employee;
            //                return View(nameof(Index), lst.ToList());
            //            }

            //        }
            //    case "DepartmentName":
            //        {
            //            if (type == 1)
            //            {
            //                IEnumerable<Employee> lst = from employee in employees
            //                                            orderby employee.EmployeeDept.DepartmentName ascending
            //                                            select employee;
            //                return View(nameof(Index), lst.ToList());
            //            }
            //            else
            //            {
            //                IEnumerable<Employee> lst = from employee in employees
            //                                            orderby employee.EmployeeDept.DepartmentName descending
            //                                            select employee;
            //                return View(nameof(Index), lst.ToList());
            //            }
            //        }
            //    case "Salary":
            //        {
            //            if (type == 1)
            //            {
            //                IEnumerable<Employee> lst = from employee in employees
            //                                            orderby employee.Salary ascending
            //                                            select employee;
            //                return View(nameof(Index), lst.ToList());
            //            }
            //            else
            //            {
            //                IEnumerable<Employee> lst = from employee in employees
            //                                            orderby employee.Salary descending
            //                                            select employee;
            //                return View(nameof(Index), lst.ToList());
            //            }
            //        }
            //    case "CreatedTime":
            //        {
            //            if (type == 1)
            //            {

            //                employees.Sort((a, b) => DateTime.Compare(a.CreatedTime, b.CreatedTime));
            //                //IEnumerable<Employee> lst = from employee in employees
            //                //                            orderby employee.CreatedTime ascending
            //                //                            select employee;
            //                return View(nameof(Index), employees);
            //            }
            //            else
            //            {
            //                IEnumerable<Employee> lst = from employee in employees
            //                                            orderby employee.CreatedTime descending
            //                                            select employee;
            //                return View(nameof(Index), lst.ToList());
            //            }
            //        }
            //    case "UpdatedTime":
            //        {
            //            if (type == 1)
            //            {
            //                IEnumerable<Employee> lst = from employee in employees
            //                                            orderby employee.UpdatedTime ascending
            //                                            select employee;
            //                return View(nameof(Index), lst.ToList());
            //            }
            //            else
            //            {
            //                IEnumerable<Employee> lst = from employee in employees
            //                                            orderby employee.UpdatedTime descending
            //                                            select employee;
            //                return View(nameof(Index), lst.ToList());
            //            }
            //        }
            //}

        }
    }
}