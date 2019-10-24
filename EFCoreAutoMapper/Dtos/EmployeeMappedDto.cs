using EFCoreAutoMapper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreAutoMapper.Dtos
{
    public class EmployeeMappedDto
    {
        public int EmployeeId { get; set; }
        public string LastName { get; set; }
        public string FirstNameSomething { get; set; }
        public string City { get; set; }
        public Date BirthDate { get; set; }
    }
}
