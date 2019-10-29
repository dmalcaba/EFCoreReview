using AutoMapper;
using EFCoreAutoMapper.MapperProfiles;
using EFCoreAutoMapper.Queries;
using System;

namespace EFCoreAutoMapper
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new MapperConfiguration(cfg => 
            {
                cfg.AddProfile<EmployeeMapperProfile>();
            });

            config.AssertConfigurationIsValid();

            var mapper = new Mapper(config);

            //new Projection(mapper).ProjectBeforeAsync().Wait();
            //new Projection(mapper).ProjectionExample().Wait();

            new Projection(mapper).ProjectAfterV2Async().Wait();

            Console.ReadLine();

        }

        private static string FormatStringValue(object value)
        {
            if (value == null) return string.Empty;
            return $"\"{value.ToString()}\"";
        }
    }
}
