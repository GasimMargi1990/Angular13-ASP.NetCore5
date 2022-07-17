using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Employee.API.BindingModel
{
    public class loginBindingModel
    {

        [Required]
        public string  Email { get; set; }
        [Required]
        public string Paswword { get; set; }
    }
}
