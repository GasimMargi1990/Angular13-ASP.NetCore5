using Employee.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee.API.Models;

namespace Employee.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly EmployeeDbContext employeeDbContext;
        public EmployeeController(EmployeeDbContext employeeDbContext)  
        {
            this.employeeDbContext = employeeDbContext;
        }

         // Get All Employees

        [HttpGet]
        public async Task <IActionResult> GetAllEmployees()
        {
         var employees = await employeeDbContext.Employees.ToListAsync();

         return Ok(employees);
        }
         
        // Get Single employee
        [HttpGet]
        [Route("{id}")]
        [ActionName("GetEmployee")]

        public async Task<IActionResult> GetEmployee([FromRoute]int id)
        {
            var employee = await employeeDbContext.Employees.
                FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null)
            {
                return Ok(employee);

            }

            return NotFound("Employee is not found");
        }


        // Add Single employee
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Models.Employee employee)

        {
            await employeeDbContext.Employees.AddAsync(employee);

            await employeeDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee),
                new { employee.Id },employee);
             


        }


        // Updating Employee
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] int id,[FromBody] Models.Employee employee)
        {
            var existingEmployee = await employeeDbContext.Employees.
                FirstOrDefaultAsync(x => x.Id == id);
            if(existingEmployee!= null)
            {
                existingEmployee.EmpName = employee.EmpName;
                existingEmployee.EmpEmail = employee.EmpEmail;

                await employeeDbContext.SaveChangesAsync();
                return Ok(existingEmployee);
            }

            return NotFound("Employee Not Found");

        }


        // Delete Employee
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            var existingEmployee = await employeeDbContext.Employees.
                FirstOrDefaultAsync(x => x.Id == id);
            if (existingEmployee != null)
            {

                employeeDbContext.Remove(existingEmployee);
                await employeeDbContext.SaveChangesAsync();
                return Ok(existingEmployee);
            }

            return NotFound("Employee Not Found");




        } 
    }
}
