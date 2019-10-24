using EFCoreReview.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Tutorial.Queries
{
    public class Tutorial
    {
        private readonly NorthwindContext _context;

        public Tutorial()
        {
            _context = new NorthwindContext();
        }

        public async Task GetAllEmployees()
        {
            var query = _context.Employees;

            var result = await query.ToListAsync();

            Console.WriteLine("\nTotal Employees {0}", result.Count);
        }
    }
}
