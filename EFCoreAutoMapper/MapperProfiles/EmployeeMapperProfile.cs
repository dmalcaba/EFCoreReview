using AutoMapper;
using EFCoreAutoMapper.Dtos;
using EFCoreAutoMapper.Models;
using EFCoreReview.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreAutoMapper.MapperProfiles
{
    public class EmployeeMapperProfile : Profile
    {
        public EmployeeMapperProfile()
        {
            CreateMap<Employees, EmployeeMappedDto>()
                .ForMember(dest => dest.FirstNameSomething, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom<DateResolver, DateTime?>(src => src.BirthDate))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom<NameValueResolver>());
        }
    }
}
