using EFCoreReview.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreReview.App
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestEFCore();
            //TestOverride();
            //ReviewQuery();
            //AggregateFunctions.ReviewSum();
            RandomQueries.ReturnOnlyOneColumn();
            Console.ReadLine();
        }

        static void TestEFCore()
        {
            using (var context = new NorthwindContext())
            {
                var categories = context.Categories.ToList();

                Console.WriteLine(); // add a blank space to separate log information from output

                foreach (var category in categories)
                {
                    Console.WriteLine($"Category {category.CategoryName} - {category.Description}");
                }
            }
        }

        /// <summary>
        /// This is to show how you can override the options and use in memory database instead
        /// </summary>
        static void TestOverride()
        {
            using (var context = GetContext())
            {
                context.Categories.Add(new Categories { CategoryName = "new", Description = "test" });

                context.SaveChanges();

                var categories = context.Categories.ToList();
                foreach (var category in categories)
                {
                    Console.WriteLine($"Category {category.CategoryId}: {category.CategoryName} - {category.Description}");
                }
            }
        }

        static NorthwindContext GetContext()
        {
            var dbOptions = new DbContextOptionsBuilder<NorthwindContext>()
                .UseInMemoryDatabase("Test").Options;

            return new NorthwindContext(dbOptions);
        }

        /// <summary>
        /// Demonstrate how you can reuse a query.
        /// The ToList initiates the Sql query.  Each query shows the resulting Sql statement
        /// Here you can see that although we have included OrderDetails, it knows not to include
        /// it if the data for the OrderDetails is not needed
        /// </summary>
        static void ReviewQuery()
        {
            using (var context = new NorthwindContext())
            {
                var query = context.Set<Orders>().Include(x => x.OrderDetails);

                var result4 = query.Select(x => x.ShipCountry).Distinct().ToList();
                /*
                info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                      Executed DbCommand (3ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                      SELECT DISTINCT [x].[ShipCountry]
                      FROM [Orders] AS [x]
                */

                var result5 = query.Select(x => x.ShipCountry).Distinct().OrderBy(x => x).ToList();
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

                var result6 = query.OrderBy(x => x.ShipCountry).Select(x => x.ShipCountry).Distinct().ToList();
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
}
