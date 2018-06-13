using EFCoreReview.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFCoreReview.App
{
    public class RandomQueries
    {
        public static void ReturnOnlyOneColumn()
        {
            using (var context = new NorthwindContext())
            {
                var result = context.Products
                    .Where(p => p.Discontinued != false)
                    .Select(g => g.ProductName)
                    .ToList();

                Console.WriteLine($"\nList of Discontinued Products:");
                foreach (var item in result)
                {
                    Console.WriteLine(item);
                }
            }
        }

        public static void TestQuery()
        {
            using (var context = new NorthwindContext())
            {
                var query = from p in context.Products
                            where p.Discontinued != false
                            select p;

                query.ToList();
            }
        }

        public static void PassingNullWhereValue()
        {
            using (var context = new NorthwindContext())
            {
                context.Products.Where(x => true).ToList();
            }
        }

    }
}
