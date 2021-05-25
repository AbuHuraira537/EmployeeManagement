using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class Employee 
    {

        public int Id { get; set; }
        [Required]
        [Display(Name ="Emp. Name")]
        public string EmployeeName { get; set; }
       
        [Display(Name ="Emp. Department")]
        public virtual Department EmployeeDept { get; set; }
        [Display(Name = "Updated")]
        public DateTime UpdatedTime { get; set; }
        [Display(Name = "Created")]
        public DateTime CreatedTime
        {
            get; set;
        }
#nullable enable
        
      
       
       [Range(1,9999999999)]
        public int? Salary { get; set; }
        
    }
}