using AutoMapper;
using EFCoreAutoMapper.Dtos;
using EFCoreAutoMapper.Models;
using EFCoreAutoMapper.Queries;
using EFCoreReview.Data.Models;
using System;

namespace EFCoreAutoMapper
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new MapperConfiguration(cfg => 
            {
                cfg.CreateMap<Employees, EmployeeMappedDto>()
                .ForMember(dest => dest.FirstNameSomething, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => new Date(src.BirthDate)));
            });
            var mapper = new Mapper(config);

            //new Projection(mapper).ProjectBeforeAsync().Wait();

            //new Projection(mapper).ProjectionExample().Wait();

            //new Projection(mapper).ProjectAfterAsync().Wait();

            var minTimeSpan = new TimeSpan(0, 0, 0);// " 00:00:00";
            var maxTimeSpan = new TimeSpan(0, 23, 59, 59, 999); // " 23:59:59";

            Console.WriteLine(maxTimeSpan.ToString());
            Console.WriteLine(minTimeSpan.ToString("g"));

            var ms = maxTimeSpan.Milliseconds;

            var test = $"2019-09-30 {maxTimeSpan.ToString("g")}";

            var date = new DateTime(2019, 10, 10, 23, 45, 22, 22);

            var result = FormatStringValue(test);

            Console.ReadLine();

        }

        private static string FormatStringValue(object value)
        {
            if (value == null) return string.Empty;
            return $"\"{value.ToString()}\"";
        }
    }
}
