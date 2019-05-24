using EFCoreReview.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace EFCoreReview.App
{
    public class UsingQuerySyntax
    {
        public void FlattenObjectDynamic()
        {
            using (var context = new NorthwindContext())
            {
                var query = GetQuery(context);
                var result = AddWhereClause(query, "ShipCountry", "Germany");

                Console.WriteLine("Record count: {0}", result.ToDynamicList().Count);
            }
        }

        public void FlattenObject()
        {
            using (var context = new NorthwindContext())
            {
                var query = from a in context.EmployeeTerritories
                            join b in context.Employees on a.EmployeeId equals b.EmployeeId
                            join c in context.Territories on a.TerritoryId equals c.TerritoryId
                            join d in context.Orders on b.EmployeeId equals d.EmployeeId
                            select new
                            {
                                a.EmployeeId,
                                b.LastName,
                                b.FirstName,
                                c.TerritoryDescription,
                                d.ShipCity,
                                d.ShipCountry
                            };

                var result = query.Where(a => a.ShipCountry == "France").OrderBy(a => a.ShipCity).ThenBy(a => a.EmployeeId);

                Console.WriteLine("Record count: {0}", result.ToList().Count);

            }
        }

        public IQueryable GetQuery(NorthwindContext context)
        {
            return from a in context.EmployeeTerritories
                   join b in context.Employees on a.EmployeeId equals b.EmployeeId
                   join c in context.Territories on a.TerritoryId equals c.TerritoryId
                   join d in context.Orders on b.EmployeeId equals d.EmployeeId
                   select new
                   {
                       a.EmployeeId,
                       b.LastName,
                       b.FirstName,
                       Desc = c.TerritoryDescription,
                       d.ShipCity,
                       d.ShipCountry
                   };
        }

        public void ComplexJoin()
        {
            using (var context = new NorthwindContext())
            {
                var query = from a in context.EmployeeTerritories
                            join b in context.Employees on a.EmployeeId equals b.EmployeeId
                            join c in context.Territories on a.TerritoryId equals c.TerritoryId
                            join d in context.Orders on b.EmployeeId equals d.EmployeeId
                            select new
                            {
                                a.EmployeeId,
                                b.LastName,
                                b.FirstName,
                                c.TerritoryDescription,
                                d.ShipCity,
                                d.ShipCountry
                            };

                var result = query.Where("ShipCountry = @0", "France").ToDynamicList();
                var result2 = AddWhereClause(query, "ShipCountry", "France");
                Console.WriteLine("Record count: {0}", result2.ToDynamicList().Count);
            }
        }

        public IQueryable AddWhereClause(IQueryable query, string column, object value)
        {
            return query.Where($"{column} = @0", value);
        }

        public IQueryable<T> AddWhereClause<T>(IQueryable<T> query, string column, object value)
        {
            return query.Where($"{column} = @0", value);
        }
    }
}
