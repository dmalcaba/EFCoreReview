using EFCoreReview.Data.Models;
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
                    .Where(p => p.Discontinued == false)
                    .Select(g => g.ProductName)
                    .ToList();

                Console.WriteLine($"\nList of Discontinued Products:");
                foreach (var item in result)
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
}
