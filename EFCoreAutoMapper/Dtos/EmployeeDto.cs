using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreAutoMapper.Dtos
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string City { get; set; }
    }
}
