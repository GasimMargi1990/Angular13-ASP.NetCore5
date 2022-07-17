using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee.API.DTO
{
    public class UserDto
    {
        public UserDto (string fullName,string email,
            string username,DateTime dateCreated)
        {
            FullName = fullName;
            Email = email;
            UserName = username;
            DateCreated = dateCreated;
        }

        public string FullName { get; set; }

        public string  Email { get; set; }

        public string UserName { get; set; }

        public DateTime DateCreated { get; set; }

        public string Token { get; set; }
    }
}
