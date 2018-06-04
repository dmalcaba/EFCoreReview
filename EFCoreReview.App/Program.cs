using EFCoreReview.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EFCoreReview.App
{
    class Program
    {
        static void Main(string[] args)
        {
            TestEFCore();
            Console.ReadLine();
        }

        static void TestEFCore()
        {
            using (var context = new NorthwindContext())
            {
                var categories = context.Categories.ToList();

                foreach (var category in categories)
                {
                    Console.WriteLine($"Category {category.CategoryName} - {category.Description}");
                }
            }
        }
    }
}
