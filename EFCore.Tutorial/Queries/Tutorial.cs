using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCore.Tutorial.Dtos;
using EFCoreReview.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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


        /// <summary>
        /// Add a where clause
        /// </summary>
        public async Task Example001()
        {
            var query = _context.Employees
                .Where(x => x.City == "London");

            var result = await query.ToListAsync();
        }

        /// <summary>
        /// Add an order by clause
        /// </summary>
        public async Task Example002()
        {
            var query = _context.Employees
                .Where(x => x.City == "London")
                .OrderBy(x => x.LastName);

            var result = await query.ToListAsync();
        }

        /// <summary>
        /// Add select top
        /// </summary>
        public async Task Example003()
        {
            var query = _context.Employees
                .Where(x => x.City == "London")
                .OrderBy(x => x.LastName);

            var result = await query.Take(3).ToListAsync();
        }

        /// <summary>
        /// Add  fetch next
        /// </summary>
        public async Task Example004()
        {
            var query = _context.Employees
                .Where(x => x.City == "London")
                .OrderBy(x => x.LastName);

            var result = await query.Skip(0).Take(3).ToListAsync();
        }

        /// <summary>
        /// </summary>
        public async Task Example005()
        {
            var query = _context
                .Orders
                .Where(x => x.ShipCountry == "Germany");

            var result = await query
                .Where(x => x.ShipCity == "Brandenburg")
                .OrderBy(x => x.OrderId)
                .Skip(0)
                .Take(10)
                .ToListAsync();
        }

        public async Task Example006()
        {
            var query = _context
                .Orders
                .Select(x => new { x.OrderId, x.OrderDate, x.ShipCountry, x.ShipCity, x.ShipName })
                .Where(x => x.ShipCountry == "Germany");

            var result = await query
                .Where(x => x.ShipCity == "Brandenburg")
                .OrderBy(x => x.OrderId)
                .Skip(0)
                .Take(10)
                .ToListAsync();
        }

        public async Task Example007()
        {
            var query = _context
                .Orders
                .Select(x => new { x.OrderId, x.OrderDate, x.ShipCountry, x.ShipCity, x.ShipName, 
                    MailingAddress = x.ShipAddress + " " + x.ShipCity + ", " + x.ShipCountry })
                .Where(x => x.ShipCountry == "Germany");

            var result = await query
                .Where(x => x.ShipCity == "Brandenburg")  /*  /shipCity= "BranderBurg"    */
                .OrderBy(x => x.OrderId) /* /orderId ASC */
                .Skip(0)
                .Take(10)
                .ToListAsync();
        }

        public async Task Example008()
        {
            var query = _context
                .Orders
                .Select(x => new {
                    x.OrderId,
                    x.OrderDate,
                    x.ShipCountry,
                    x.ShipCity,
                    x.ShipName,
                    MailingAddress = x.ShipAddress + " " + x.ShipCity + ", " + x.ShipCountry
                })
                .Where(x => x.ShipCountry == "Germany");

            var result = await query
                .Where(x => x.MailingAddress.Contains("Brandenburg") )
                .OrderBy(x => x.OrderId)
                .Skip(0)
                .Take(10)
                .ToListAsync();
        }

        public async Task Example009()
        {
            var query = _context
                .Orders
                .Select(x => new {
                    x.OrderId,
                    x.OrderDate,
                    x.ShipCountry,
                    x.ShipCity,
                    x.ShipName,
                    MailingAddress = GetMailingAddress(x.ShipAddress, x.ShipCity, x.ShipCountry)
                })
                .Where(x => x.ShipCountry == "Germany");

            var result = await query
                .Where(x => x.MailingAddress.Contains("Brandenburg"))
                .OrderBy(x => x.OrderId)
                .Skip(0)
                .Take(10)
                .ToListAsync();
        }

        public async Task Example010()
        {
            var query = _context
                .Orders
                .Select(x => new
                {
                    x.OrderId,
                    x.OrderDate,
                    x.ShipCountry,
                    x.ShipCity,
                    x.ShipRegion,
                    x.ShipName,
                    x.Freight,
                    MailingAddress = x.ShipAddress + " " + x.ShipCity + ", " + x.ShipCountry
                    //MailingAddress = GetMailingAddress(x.ShipAddress, x.ShipCity, x.ShipCountry)
                });
                //.Where(x => x.ShipCountry == "Germany");

            var result = await query
                .Where(x => x.MailingAddress.Contains("RIO") && x.ShipRegion.ToLower() == "RJ".ToLower())
                .OrderBy(x => x.OrderId)
                .Skip(0)
                .Take(10)
                .ToListAsync();
        }


        public async Task Example010a()
        {
            var query = _context
                .Orders
                .Select(x => new
                {
                    x.OrderId,
                    x.OrderDate,
                    x.ShipCountry,
                    x.ShipCity,
                    x.ShipRegion,
                    x.ShipName,
                    x.Freight,
                    MailingAddress = x.ShipAddress + " " + x.ShipCity + ", " + x.ShipCountry
                    //MailingAddress = GetMailingAddress(x.ShipAddress, x.ShipCity, x.ShipCountry)
                }).ToList();
                
            var query2 = query.AsQueryable();
            //.Where(x => x.ShipCountry == "Germany");

            var result = query2
                .Where(x => x.MailingAddress.Contains("Rio") && x.ShipRegion.ToLower() == "rj")
                .OrderBy(x => x.OrderId)
                .Skip(0)
                .Take(10)
                .ToList();
        }

        private string GetMailingAddress(string address, string city, string country)
            => address + " " + city + ", " + country;

        /// <summary>
        /// http://docs.automapper.org/en/stable/Queryable-Extensions.html
        /// </summary>
        public async Task Example011(IMapper mapper)
        {
            var query = _context
                .Employees
                //.Select(x => new { x.EmployeeId, x.FirstName, x.LastName, x.Address });
                //.Select(x => new EmployeeDto { EmployeeId = x.EmployeeId, FirstName = x.FirstName, LastName = x.LastName, City = x.City, Address = x.Address });
                .ProjectTo<EmployeeDto>(mapper.ConfigurationProvider);

            var result = await query.Where(x=>x.FullName.Contains("King")).ToListAsync();
        }

        public async Task Example012(IMapper mapper)
        {
            var query = _context
                .Employees
                .ProjectTo<EmployeeDto>(mapper.ConfigurationProvider);

            var result = await query.Where(x => x.FullName.Contains("King")).ToListAsync();
        }

        public async Task Example013(IMapper mapper)
        {
            var query = _context
                .Employees;

            var result = await query
                //.Where(x => x.FullName.Contains("King"))
                .Where(x => x.BirthDate.Value < new DateTime(2018, 1, 1))
                .ProjectTo<EmployeeGridDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}

