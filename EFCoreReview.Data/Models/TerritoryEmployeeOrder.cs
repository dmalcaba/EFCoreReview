using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreReview.Data.Models
{
    public class TerritoryEmployeeOrder
    {
        public int EmployeeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string TerritoryDescription { get; set; }
        public string ShipCity { get; set; }
        public string ShipCountry { get; set; }
    }
}
