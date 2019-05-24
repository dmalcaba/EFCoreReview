using EFCoreReview.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFCoreReview.App
{
    public class UsingDbQuery
    {
        public void UsingFromSql()
        {
            /*
                info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                      Executed DbCommand (41ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                      SELECT [a].[EmployeeId], [a].[FirstName], [a].[LastName], [a].[ShipCity], [a].[ShipCountry], [a].[TerritoryDescription]
                      FROM (
                          select
                                a.EmployeeID, b.LastName, b.FirstName,
                                 c.TerritoryDescription, d.ShipCity, d.ShipCountry
                          from EmployeeTerritories a
                                inner join Employees b
                                        on a.EmployeeID = b.EmployeeID
                                inner join Territories c
                                        on a.TerritoryID = c.TerritoryID
                                inner join Orders d
                                        on a.EmployeeID = d.EmployeeID
                      ) AS [a]
                      WHERE [a].[ShipCountry] = N'France'
                      ORDER BY [a].[TerritoryDescription]
            */


            using (var context = new NorthwindContext())
            {
                var query = context.TerritoryEmployeeOrders.FromSql(
                    @"select 
	a.EmployeeID, b.LastName, b.FirstName,
	 c.TerritoryDescription, d.ShipCity, d.ShipCountry
from EmployeeTerritories a
	inner join Employees b
		on a.EmployeeID = b.EmployeeID
	inner join Territories c
		on a.TerritoryID = c.TerritoryID
	inner join Orders d
		on a.EmployeeID = d.EmployeeID");


                var result = query.Where(a => a.ShipCountry == "France").OrderBy(a => a.TerritoryDescription).ToList();

                Console.WriteLine("Count {0}", result.Count);

            }
        }
    }
}
