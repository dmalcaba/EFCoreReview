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
            //TestEFCore();
            TestOverride();
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
            var dbOptions = new DbContextOptionsBuilder<NorthwindContext>()
                .UseInMemoryDatabase("Test").Options;

            using (var context = new NorthwindContext(dbOptions))
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
    }
}
