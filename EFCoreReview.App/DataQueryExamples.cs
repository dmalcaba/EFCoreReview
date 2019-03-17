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

            PrintResultsCategories(categories);
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

            CategoryPrinter(category);
        }

        /// <summary>
        /// Example of using Take
        /// </summary>
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

            ** This is incorrect.  OrderBy should come first before Take.
            ** By doing OrderBy first, the resulting SQL statement is more correct
            ** and what you would expect

            var categories = await _context.Categories
                    .Take(count)
                    .OrderBy(a => a.CategoryName)
                    .ToListAsync();
            */

            /*
            info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                  Executed DbCommand (85ms) [Parameters=[@__p_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
                  SELECT TOP(@__p_0) [a].[CategoryID], [a].[CategoryName], [a].[Description], [a].[Picture]
                  FROM [Categories] AS [a]
                  ORDER BY [a].[CategoryName]
            */

            var categories = await _context.Categories
                    .OrderBy(a => a.CategoryName)
                    .Take(count)
                    .ToListAsync();

            PrintResultsCategories(categories);
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

            PrintResults(categories, a => Console.WriteLine($"Category {a.Name} - {a.Desc}"));
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

            var products = await _context.Products
                //.Include(a => a.Category).Take(10)
                .ToListAsync();

          //  PrintResults(products, a => Console.WriteLine($"Products: {a.ProductName} {a.Category.CategoryName}"));

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

            var categories = await _context.Categories
                .OrderBy(a => a.CategoryName)
           //     .Include(a => a.Products)
                .ToListAsync();

            PrintResults(categories, a => 
            {
                Console.WriteLine();
                Console.WriteLine($"{a.CategoryName}");
                Console.WriteLine($"{a.Description}");
                foreach (var product in a.Products.Take(5))
                {
                    Console.WriteLine($"\t{product.UnitPrice:c0} \t\t{product.ProductName}");
                }
            });

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

        /// <summary>
        /// Regardless of where the order by is, the resulting Sql is the same
        /// </summary>
        public async Task GetEmployeeWhereAndOrderBy()
        {
            /*
            info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                  Executed DbCommand (68ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                  SELECT [a].[EmployeeID], [a].[Address], [a].[BirthDate], [a].[City], [a].[Country], [a].[Extension], [a].[FirstName], [a].[HireDate], [a].[HomePhone], [a].[LastName], [a].[Notes], [a].[Photo], [a].[PhotoPath], [a].[PostalCode], [a].[Region], [a].[ReportsTo], [a].[Title], [a].[TitleOfCourtesy]
                  FROM [Employees] AS [a]
                  WHERE [a].[TitleOfCourtesy] = N'Mr.'
                  ORDER BY [a].[Title]
            */

            var result = await _context.Employees
                .Where(a => a.TitleOfCourtesy == "Mr.")
                .OrderBy(a => a.Title)
                .ToListAsync();

            PrintResults(result, a => Console.WriteLine($"Employee : {a.TitleOfCourtesy} {a.FirstName} {a.LastName}, {a.Title}"));

            /*
            info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                  Executed DbCommand (58ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                  SELECT [a].[EmployeeID], [a].[Address], [a].[BirthDate], [a].[City], [a].[Country], [a].[Extension], [a].[FirstName], [a].[HireDate], [a].[HomePhone], [a].[LastName], [a].[Notes], [a].[Photo], [a].[PhotoPath], [a].[PostalCode], [a].[Region], [a].[ReportsTo], [a].[Title], [a].[TitleOfCourtesy]
                  FROM [Employees] AS [a]
                  WHERE [a].[TitleOfCourtesy] = N'Mr.'
                  ORDER BY [a].[Title]
            */

            var result2 = await _context.Employees
                .OrderBy(a => a.Title)
                .Where(a => a.TitleOfCourtesy == "Mr.")
                .ToListAsync();

            PrintResults(result, a => Console.WriteLine($"Employee : {a.TitleOfCourtesy} {a.FirstName} {a.LastName}, {a.Title}"));
        }

        /// <summary>
        /// Example of selecting a distinct field with where clause and order by.
        /// Notice the difference between the placement of the order by method
        /// and the resulting Sql query.
        /// Placement of the OrderBy matters, although the results are the same,
        /// the second statement is efficient.
        /// </summary>
        public async Task GetDistinctEmployeeTitle()
        {
            /*
            info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                  Executed DbCommand (57ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                  SELECT [t].[Title]
                  FROM (
                      SELECT DISTINCT [a].[Title]
                      FROM [Employees] AS [a]
                      WHERE [a].[TitleOfCourtesy] = N'Mr.'
                  ) AS [t]
                  ORDER BY [t].[Title]
             */

            var result = await _context.Employees
                .Where(a => a.TitleOfCourtesy == "Mr.")
                .Select(a => a.Title)
                .Distinct()
                .OrderBy(a => a)
                .ToListAsync();

            PrintResults(result, a => Console.WriteLine($"Employee Title : {a}"));

            /*
            info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                  Executed DbCommand (56ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                  SELECT DISTINCT [a].[Title]
                  FROM [Employees] AS [a]
                  WHERE [a].[TitleOfCourtesy] = N'Mr.'
                  ORDER BY [a].[Title]
            */

            var result2 = await _context.Employees
                .Where(a => a.TitleOfCourtesy == "Mr.")
                .Select(a => a.Title)
                .OrderBy(a => a)
                .Distinct()
                .ToListAsync();

            PrintResults(result2, a => Console.WriteLine($"Employee Title : {a}"));
        }

        /// <summary>
        /// Here is an example of a use of multiple order by, mixing it with asc or desc.
        /// You are also able to order by a column that is not part of the select. To do this
        /// you'll add the orderby first then the select
        /// </summary>
        public async Task GetEmployeeWithMultipleOrderBy()
        {
            /*
            info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                  Executed DbCommand (69ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                  SELECT [a].[LastName], [a].[FirstName]
                  FROM [Employees] AS [a]
                  ORDER BY [a].[LastName], [a].[FirstName], [a].[BirthDate] DESC
            */

            var result = await _context.Employees
                .OrderBy(a => a.LastName)
                    .ThenBy(a => a.FirstName)
                    .ThenByDescending(a => a.BirthDate)
                .Select(e => new { e.LastName, e.FirstName })
                .ToListAsync();

            PrintResults(result, a => Console.WriteLine($"Employee : {a.LastName}, {a.FirstName}"));
        }


        #region Output
        private void PrintResultsCategories(List<Categories> categories)
        {
            PrintResults(categories, CategoryPrinter);
        }

        private static void PrintResults<T>(List<T> items, Action<T> printAction)
        {
            Console.WriteLine(); // add a blank space to separate log information from output

            foreach (var item in items)
            {
                printAction(item);
            }

            Console.WriteLine();
        }

        private void CategoryPrinter(Categories category)
        {
            Console.WriteLine($"Category {category.CategoryName} - {category.Description}");
        } 
        #endregion
    }
}
