using AutoMapper;
using EFCoreAutoMapper.Queries;
using System;

namespace EFCoreAutoMapper
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new MapperConfiguration(cfg => { });
            var mapper = new Mapper(config);

            new Projection(mapper).AutoProjectionAsync().Wait();
        }
    }
}
