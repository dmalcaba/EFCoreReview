using EFCoreReview.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreReview.App
{
    public class JoinOperators
    {
        private readonly NorthwindContext _context;

        public JoinOperators()
        {
            _context = new NorthwindContext();
        }

        public async Task GroupJoin()
        {
            /*
            info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                  Executed DbCommand (67ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                  SELECT [a].[SupplierID], [a].[Address], [a].[City] AS [Key], [a].[CompanyName], [a].[ContactName], [a].[ContactTitle], [a].[Country], [a].[Fax], [a].[HomePage], [a].[Phone], [a].[PostalCode], [a].[Region], [b].[ProductID], [b].[CategoryID], [b].[Discontinued], [b].[ProductName], [b].[QuantityPerUnit], [b].[ReorderLevel], [b].[SupplierID], [b].[UnitPrice], [b].[UnitsInStock], [b].[UnitsOnOrder]
                  FROM [Suppliers] AS [a]
                  LEFT JOIN [Products] AS [b] ON [a].[SupplierID] = [b].[SupplierID]
                  ORDER BY [Key], [a].[SupplierID]
            */

            var query =
                from a in _context.Suppliers
                join b in _context.Products on a.SupplierId equals b.SupplierId into supProd
                select new { Key = a.City, Items = supProd };

            var result = await query.OrderBy(a => a.Key).ToListAsync();

            foreach (var item in result)
            {
                Console.WriteLine(item.Key + ":");
                foreach (var product in item.Items)
                {
                    Console.WriteLine("   " + product.ProductName);
                }
            }

        }
    }
}
