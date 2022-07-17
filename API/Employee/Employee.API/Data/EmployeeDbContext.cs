using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Employee.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Employee.API.Data
{
    public class EmployeeDbContext : IdentityDbContext<AppUser,IdentityRole,string>


    {
        public EmployeeDbContext (DbContextOptions options) : base(options)

        {

        }

        public DbSet<Models.Employee>  Employees { get; set; }
    }

}
 