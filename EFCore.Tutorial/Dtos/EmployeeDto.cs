using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.Tutorial.Dtos
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? Birthdate { get; set; }
        public string BirthdateString { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public string City { get; set; }
    }
}
