using EFCoreReview.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreReview.App
{
    /// <summary>
    /// Method-Based Query Syntax Examples: Aggregate Operators
    /// </summary>
    public class AggregateFunctions
    {
        public static async Task CountSampleAsync()
        {
            /*
                info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                      Executed DbCommand (30ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                      SELECT COUNT(*)
                      FROM [Customers] AS [c]
            */

            using (var context = new NorthwindContext())
            {
                //
                var result = context.Customers
                    .Count();

                // Async   
                var result2 = await context.Customers.CountAsync();

            }

            /*
                info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                        Executed DbCommand (12ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                        SELECT COUNT(*)
                        FROM [Customers] AS [a]
                        WHERE [a].[Country] = N'Spain'
            */
            using (var context = new NorthwindContext())
            {
                var spainCustomer = await context.Customers.Where(a => a.Country == "Spain").CountAsync();

                //alternate
                var spainCustomerAlt = await context.Customers.CountAsync(a => a.Country == "Spain");
            }
        }

        public static async Task MaxSampleAsync()
        {
            /*
                info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                      Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                      SELECT MAX([a].[Quantity])
                      FROM [Order Details] AS [a]
                      WHERE [a].[OrderID] = 10248
            */

            using (var context = new NorthwindContext())
            {
                var spainCustomer = await context.OrderDetails
                    .Where(a => a.OrderId == 10248)
                    .MaxAsync(a => a.Quantity);
            }
        }

        public static async Task SumSampleAsync()
        {
            /*
                info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                      SELECT SUM([a].[Quantity] * [a].[UnitPrice])
                      FROM [Order Details] AS [a]
                      WHERE [a].[OrderID] = 10248
            */

            using (var context = new NorthwindContext())
            {
                var spainCustomer = await context.OrderDetails
                    .Where(a => a.OrderId == 10248)
                    .SumAsync(a => a.Quantity * a.UnitPrice);
            }
        }

        public static void ReviewSum()
        {
            using (var context = new NorthwindContext())
            {
                var result = context.Set<OrderDetails>()
                    .GroupBy(o => o.Order.Customer)
                    .Select(g => new {
                        g.Key.ContactName,
                        Sum = g.Sum(od => (od.Quantity * od.UnitPrice))
                    }).ToList();

                foreach (var item in result)
                {
                    Console.WriteLine($"Customer {item.ContactName} - Amount $ {item.Sum}");
                }
            }
            /*
                -- The above code produces this query
                -- It is not really doing a group nor a sum as expected.
             
            info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                  Executed DbCommand (39ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                  SELECT [o0].[OrderID], [o0].[ProductID], [o0].[Discount], [o0].[Quantity], [o0].[UnitPrice], 
                        [o.Order.Customer0].[CustomerID], [o.Order.Customer0].[Address], [o.Order.Customer0].[City], 
                        [o.Order.Customer0].[CompanyName], [o.Order.Customer0].[ContactName], [o.Order.Customer0].[ContactTitle], 
                        [o.Order.Customer0].[Country], [o.Order.Customer0].[Fax], [o.Order.Customer0].[Phone], 
                        [o.Order.Customer0].[PostalCode], [o.Order.Customer0].[Region]
                  FROM [Order Details] AS [o0]
                  INNER JOIN [Orders] AS [o.Order0] ON [o0].[OrderID] = [o.Order0].[OrderID]
                  LEFT JOIN [Customers] AS [o.Order.Customer0] ON [o.Order0].[CustomerID] = [o.Order.Customer0].[CustomerID]
              */
        }


    }
}
