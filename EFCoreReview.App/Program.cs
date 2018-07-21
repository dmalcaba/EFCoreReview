using EFCoreReview.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreReview.App
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestOverride();
            //AggregateFunctions.ReviewSum();
            //RandomQueries.ReturnOnlyOneColumn();
            //RandomQueries.TestQuery();
            //RandomQueries.PassingNullWhereValue();

            //new DataQueryExamples().GetCategoryByIdAsync(1).Wait();
            //new DataQueryExamples().GetFirstCategoryOrderByNameAsync().Wait();
            //new DataQueryExamples().GetCategoriesTakeAsync(5).Wait();
            //new DataQueryExamples().GetDistinctEmployeeTitle().Wait();
            new DataQueryExamples().GetEmployeeWhereAndOrderBy().Wait();

        }

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
