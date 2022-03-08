using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5.Dtos.Employee
{
    public class EmployeeDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string DepartmetName { get; set; }
        public string PhotosPath { get; set; }

    }
}
