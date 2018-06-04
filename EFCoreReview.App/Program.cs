using EFCoreReview.Data.Models;
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

                Console.WriteLine(); // add a blank space to separate log information from output

                foreach (var category in categories)
                {
                    Console.WriteLine($"Category {category.CategoryName} - {category.Description}");
                }
            }
        }
    }
}
