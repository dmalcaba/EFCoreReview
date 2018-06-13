using EFCoreReview.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreReview.App
{
    public class DataQueryExamples
    {
        private readonly NorthwindContext _context;

        public DataQueryExamples()
        {
            _context = new NorthwindContext();
        }

        public async Task ProductCategoryQueryAsync()
        {
            var data = await _context.Categories.Include(x => x.Products).ToListAsync();
        }

        public async Task GetAllCategoriesAsync()
        {
            /*
            info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                  Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                  SELECT [c].[CategoryID], [c].[CategoryName], [c].[Description], [c].[Picture]
                  FROM [Categories] AS [c]
            */

            var categories = await _context.Categories.ToListAsync();

            PrintResults(categories);
        }

        public async Task GetFirstCategoryAsync()
        {
            /*
            info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                  Executed DbCommand (56ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                  SELECT TOP(1) [c].[CategoryID], [c].[CategoryName], [c].[Description], [c].[Picture]
                  FROM [Categories] AS [c]
             */

            var category = await _context.Categories.FirstOrDefaultAsync();
            Console.WriteLine($"\n {category.CategoryId}. {category.CategoryName} - {category.Description}");
        }

        public async Task GetFirstCategoryOrderByNameAsync()
        {
            /*
            info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                  Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                  SELECT TOP(1) [a].[CategoryID], [a].[CategoryName], [a].[Description], [a].[Picture]
                  FROM [Categories] AS [a]
                  ORDER BY [a].[CategoryName]
             */

            var category = await _context.Categories.OrderBy(a => a.CategoryName).FirstOrDefaultAsync();
            Console.WriteLine($"\n {category.CategoryId}. {category.CategoryName} - {category.Description}");
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task GetCategoryByIdAsync(int id)
        {
            /*
            info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                  Executed DbCommand (60ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                  SELECT TOP(2) [a].[CategoryID], [a].[CategoryName], [a].[Description], [a].[Picture]
                  FROM [Categories] AS [a]
                  WHERE [a].[CategoryID] = 1
             */

            var category = await _context.Categories.SingleOrDefaultAsync(a => a.CategoryId == id);
        }

        /// <summary>
        /// Example of using Take
        /// TakeLast and TakeWhile is currently not support (EF 2.1)
        /// </summary>
        /// <param name="count"></param>
        public async Task GetCategoriesTakeAsync(int count)
        {
            /*
            info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                  Executed DbCommand (116ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
                  SELECT [t].[CategoryID], [t].[CategoryName], [t].[Description], [t].[Picture]
                  FROM (
                      SELECT TOP(@__p_0) [c].[CategoryID], [c].[CategoryName], [c].[Description], [c].[Picture]
                      FROM [Categories] AS [c]
                  ) AS [t]
                  ORDER BY [t].[CategoryName]
             */

            var categories = await _context.Categories
                    .Take(count)
                    .OrderBy(a => a.CategoryName)
                    .ToListAsync();

            PrintResults(categories);
        }

        /// <summary>
        /// This example uses <c>Select</c>
        /// to only select the columns you need.
        /// </summary>
        /// <returns></returns>
        public async Task GetAllCategoriesWithSelectFieldsAsync()
        {
            /*
            info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                  Executed DbCommand (78ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                  SELECT [p].[CategoryName] AS [Name], [p].[Description] AS [Desc]
                  FROM [Categories] AS [p]
            */

            var categories = await _context.Categories
                .Select(p => new { Name = p.CategoryName, Desc = p.Description })
                .ToListAsync();

            Console.WriteLine(); // add a blank space to separate log information from output

            foreach (var category in categories)
            {
                Console.WriteLine($"Category {category.Name} - {category.Desc}");
            }
        }

        /// <summary>
        /// The first query is a Many (Products) to One (Category)
        /// The second query is a One (Category) to Many (Products)
        /// </summary>
        /// <returns></returns>
        public async Task GetAllCategoriesIncludingProductsAsync()
        {
            /*
            info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                  Executed DbCommand (66ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                  SELECT [a].[ProductID], [a].[CategoryID], [a].[Discontinued], [a].[ProductName], [a].[QuantityPerUnit], [a].[ReorderLevel], [a].[SupplierID], [a].[UnitPrice], [a].[UnitsInStock], [a].[UnitsOnOrder], [a.Category].[CategoryID], [a.Category].[CategoryName], [a.Category].[Description], [a.Category].[Picture]
                  FROM [Products] AS [a]
                  LEFT JOIN [Categories] AS [a.Category] ON [a].[CategoryID] = [a.Category].[CategoryID]
            */

            var result = await _context.Products
                .Include(a => a.Category)
                .ToListAsync();

            /*
            info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                  Executed DbCommand (65ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                  SELECT [a].[CategoryID], [a].[CategoryName], [a].[Description], [a].[Picture]
                  FROM [Categories] AS [a]
                  ORDER BY [a].[CategoryID]
            info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                  Executed DbCommand (5ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                  SELECT [a.Products].[ProductID], [a.Products].[CategoryID], [a.Products].[Discontinued], [a.Products].[ProductName], [a.Products].[QuantityPerUnit], [a.Products].[ReorderLevel], [a.Products].[SupplierID], [a.Products].[UnitPrice], [a.Products].[UnitsInStock], [a.Products].[UnitsOnOrder]
                  FROM [Products] AS [a.Products]
                  INNER JOIN (
                      SELECT [a0].[CategoryID]
                      FROM [Categories] AS [a0]
                  ) AS [t] ON [a.Products].[CategoryID] = [t].[CategoryID]
                  ORDER BY [t].[CategoryID]
             */

            var result2 = await _context.Categories
                .Include(a => a.Products)
                .ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task Aggregates()
        {
            var query = await _context.OrderDetails
                .Include(a => a.Product)
                .GroupBy(o => new { o.OrderId })
                .Select(g => new
                    {
                        g.Key.OrderId, 
                        Sum = g.Sum(o => o.Quantity * o.UnitPrice),
                        Min = g.Min(o => o.Quantity * o.UnitPrice),
                        Max = g.Max(o => o.Quantity * o.UnitPrice),
                        Avg = g.Average(o => o.Quantity * o.UnitPrice)
                    })
                .ToListAsync();
        }

        private static void PrintResults(List<Categories> categories)
        {
            Console.WriteLine(); // add a blank space to separate log information from output

            foreach (var category in categories)
            {
                Console.WriteLine($"Category {category.CategoryName} - {category.Description}");
            }
        }

        /// <summary>
        /// Demonstrate how you can reuse a query.
        /// The ToListAsync initiates the Sql query.  Each query shows the resulting Sql statement
        /// Here you can see that although we have included OrderDetails, it knows not to include
        /// it if the data for the OrderDetails is not needed
        /// </summary>
        public async Task QueryReuseAsync()
        {
                var query = _context.Set<Orders>().Include(x => x.OrderDetails);

                var result4 = await query.Select(x => x.ShipCountry).Distinct().ToListAsync();
                /*
                info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                      Executed DbCommand (3ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                      SELECT DISTINCT [x].[ShipCountry]
                      FROM [Orders] AS [x]
                */

                var result5 = await query.Select(x => x.ShipCountry).Distinct().OrderBy(x => x).ToListAsync();
                /*
                info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                      Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                      SELECT [t].[ShipCountry]
                      FROM (
                          SELECT DISTINCT [x].[ShipCountry]
                          FROM [Orders] AS [x]
                      ) AS [t]
                      ORDER BY [t].[ShipCountry]
                */

                var result6 = await query.OrderBy(x => x.ShipCountry).Select(x => x.ShipCountry).Distinct().ToListAsync();
                /*
                info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                      Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                      SELECT DISTINCT [x].[ShipCountry]
                      FROM [Orders] AS [x]
                      ORDER BY [x].[ShipCountry]
                */
        }
    }
}
