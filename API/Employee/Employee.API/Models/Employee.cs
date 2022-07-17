using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Employee.API.Models
{  
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        public string EmpName { get; set; }

        public string   EmpEmail { get; set; }
    }
}
