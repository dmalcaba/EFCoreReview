using EFCoreReview.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EFCoreReview.App
{
    public class UsingQueryable
    {
        public void QueryEmployee()
        {
            using (var context = new NorthwindContext())
            {
                var query = context.EmployeeTerritories.Include(a => a.Employee);

                Expression<Func<EmployeeTerritories, bool>> expression = (x => x.Employee.EmployeeId == 2);

                // not supported
                Expression<Func<EmployeeTerritories, int, bool>> expression2 = ( (x,y) => x.Employee.EmployeeId == y);

                Expression<Func<EmployeeTerritories, int>> expression3 = x => x.Employee.EmployeeId;


               // var queryOrder = Order(query, "EmployeeId", "ASC");

                //var query3 = queryOrder.Where(expression).OrderBy(a => a.Employee.Orders);

                var queryOrderdynamic = query.OrderByDynamic("EmployeeId", QueryableExtensions.Order.Asc);

                var result = queryOrderdynamic.ToList();

                Console.WriteLine(result.Count());
            }
        }

        public IQueryable<EmployeeTerritories> Order(IQueryable<EmployeeTerritories> query, string column, string direction)
        {
            Expression<Func<EmployeeTerritories, object>> keySelector;

            switch (column)
            {
                case "EmployeeId":
                    keySelector = a => a.EmployeeId;
                    break;
                default:
                    keySelector = a => a.TerritoryId;
                    break;
            }

            var order = direction == "ASC" ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);

            return order;
        }

        public void DBContextEntryLoad()
        {
            using (var context = new NorthwindContext())
            {
                var employeeTerritories = context.EmployeeTerritories.First();

                context.Entry(employeeTerritories).Reference(a => a.Employee).Load();
                context.Entry(employeeTerritories).Reference(a => a.Territory).Query().Load();
            }
        }
    }
}
