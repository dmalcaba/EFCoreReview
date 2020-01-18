using AutoMapper;
using EFCore.Tutorial.MapperProfiles;
using System;

namespace EFCore.Tutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            //new Queries.Tutorial().GetAllEmployees().Wait();
            //new Queries.Tutorial().Example001().Wait();
            //new Queries.Tutorial().Example002().Wait();
            //new Queries.Tutorial().Example003().Wait();
            //new Queries.Tutorial().Example004().Wait();
            //new Queries.Tutorial().Example005().Wait();
            //new Queries.Tutorial().Example006().Wait();
            //new Queries.Tutorial().Example007().Wait();
            //new Queries.Tutorial().Example008().Wait();

            //new Queries.Tutorial().Example009().Wait();
           // new Queries.Tutorial().Example010().Wait();
           // new Queries.Tutorial().Example010a().Wait();

            var mapper = GetMapper();

            //new Queries.Tutorial().Example011(mapper).Wait();
            //new Queries.Tutorial().Example012(mapper).Wait();
            new Queries.Tutorial().Example013(mapper).Wait();
        }

        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EmployeeMapperProfile>();
            });

            var mapper = new Mapper(config);

            return mapper;
        }

    }
}
