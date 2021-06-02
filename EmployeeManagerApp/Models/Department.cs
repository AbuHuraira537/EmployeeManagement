using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class Department
    {
        public Department()
        {
            Employees = new List<Employee>();
        }
        public int Id { get; set; }
        [Required]
        [Display(Name = "Dept. Name")]
        public string DepartmentName { get; set; }
        [Display(Name = "Created")]
        public DateTime CreatedTime { get; set; }
        [Display(Name = "Updated")]
        public DateTime UpdatedTime { get; set; }
        [JsonIgnore]
        public virtual ICollection<Employee> Employees { get; set; }
#nullable enable
        

    }
}
