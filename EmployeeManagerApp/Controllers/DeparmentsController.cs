using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagerApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagerApp.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class DeparmentsController : Controller
    {
        private readonly EmployeeContext context;
        private readonly IUnitOfWork unitOfWork;

        public DeparmentsController(EmployeeContext context,IUnitOfWork unitOfWork)
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
        }
        // GET: Deparments
        public async Task<ActionResult> Index(string search)
        {
           
            //List<Department> d = await context.Set<Department>().Skip(2).Take(5).ToListAsync();
            //return View(d);
            IEnumerable<Department> departments;
            if (search ==null)
            {
                //departments = await context.Set<Department>().ToListAsync();
                departments = await unitOfWork.Departments.GetAll();
                return View(departments.ToList());
            }
            //departments = await context.Set<Department>().Include(emp => emp.Employees).Where(dept => dept.DepartmentName.Contains(search)).ToListAsync();
            departments = await unitOfWork.Departments.SearchDepartment(search);
             return View(departments.ToList());
        }

        // GET: Deparments/Details/5
        public async Task<ActionResult> Details(int id)
        {
            //Department department = await context.Set<Department>().Include(d => d.Employees).FirstOrDefaultAsync(d=>d.Id==id);
            Department department = await unitOfWork.Departments.Get(id);
            return View(department);
        }

        // GET: Deparments/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Deparments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Department department)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    // department.CreatedTime = DateTime.Now;
                    // department.UpdatedTime = DateTime.Now;
                    //await context.Set<Department>().AddAsync(department);
                    await unitOfWork.Departments.Add(department);
                    await unitOfWork.Save();
                    //await context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Please enter valid values");
                return View();
              
            }
            catch
            {
                return View();
            }
        }

        // GET: Deparments/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            // return View(await context.Set<Department>().FirstOrDefaultAsync(dept=>dept.Id==id));
            return View(await unitOfWork.Departments.Get(id));
        }

        // POST: Deparments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //department.UpdatedTime = DateTime.Now;
                    //context.Entry(department).State = EntityState.Modified;
                    //await context.SaveChangesAsync();
                   await unitOfWork.Departments.UpdateDepartment(department);
                    await unitOfWork.Save();
                    return RedirectToAction(nameof(Index));
                }
                // TODO: Add update logic here
                ModelState.AddModelError("", "Please enter valid values");
                return View(department);
               
            }
            catch
            {
                return View();
            }
        }

        // GET: Deparments/Delete/5
        

        // POST: Deparments/Delete/5
       
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                //Department department = await context.Set<Department>().Include(dept=>dept.Employees).FirstOrDefaultAsync(dept=>dept.Id==id);
                //List<Employee> employees = await context.Set<Employee>().Where(emp => emp.EmployeeDept.Id == id).ToListAsync();
                //context.RemoveRange(employees);
                //context.Set<Department>().Remove(department);

                //await context.SaveChangesAsync();
                await unitOfWork.Departments.Remove(id);
                await unitOfWork.Save();
                return RedirectToAction(nameof(Index));
                
            }
            catch
            {
                return View();
            }
        }
    }
}