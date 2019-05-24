using EFCoreReview.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace EFCoreReview.App
{
    /// <summary>
    /// This is to review the use of a Nuget package that allows for dynamically
    /// creating where and sort clauses
    /// </summary>
    public class UsingDynamicLinq
    {
        public void QueryEmployeeTerritories()
        {

            using (var context = new NorthwindContext())
            {
                var query = context.EmployeeTerritories
                        .Include(a => a.Territory)
                        .Include(a => a.Employee)
                            .ThenInclude(b => b.Orders)
                        .AsQueryable();

                int value = 2;

                LambdaExpression e = DynamicExpressionParser.ParseLambda(
                    typeof(EmployeeTerritories), typeof(bool),
                    "EmployeeId = @0", value);

                query.Where("EmployeeId = @0", value);

                LambdaExpression f = DynamicExpressionParser.ParseLambda(
                    typeof(Orders), typeof(bool),
                    "ShipCountry = @0", "Germany");

                //var result = query.Where("Employee.TitleOfCourtesy = @0", "Mr.").ToDynamicList();

                //var result = query.Where("Employee.Order.Count >= @0", 1).ToDynamicList();

                var result = query.Where(e).ToDynamicList();

                //var result = query.ToList();

                Console.WriteLine(result.Count);

            }
        }
    }
}
