using System;

namespace EFCore.Tutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            new Queries.Tutorial().GetAllEmployees().Wait();

        }
    }
}
