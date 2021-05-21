﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class EmployeeContext:DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options):base(options)
        {
        }
        DbSet<Department> Departments { get; set; }
        DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            
            modelBuilder.Entity<Employee>()
                 .HasOne(emp => emp.EmployeeDept)
                 .WithMany(dept => dept.Employees);
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Department>().ToTable("Departments");
        }
    }
}
