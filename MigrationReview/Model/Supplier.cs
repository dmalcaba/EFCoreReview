using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MigrationReview.Model
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string CompanyName { get; set; }
    }
}
