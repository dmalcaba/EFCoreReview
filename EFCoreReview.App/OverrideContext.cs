using EFCoreReview.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EFCoreReview.App
{
    public class OverrideContext
    {
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
    }
}
