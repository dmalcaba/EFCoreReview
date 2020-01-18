using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.Tutorial.Dtos
{
    public class EmployeeGridDto
    {
        public int EmployeeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string BirthdateString { get; set; }
        public string City { get; set; }
    }
}
