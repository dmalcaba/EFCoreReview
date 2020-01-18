using AutoMapper;
using EFCore.Tutorial.Dtos;
using EFCoreReview.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.Tutorial.MapperProfiles
{
    public class EmployeeMapperProfile : Profile
    {
        public EmployeeMapperProfile()
        {
            CreateMap<Employees, EmployeeDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.LastName + ", " + src.FirstName))
                .ForMember(dest => dest.BirthdateString, opt => opt.MapFrom(src => src.BirthDate.Value.ToString("dddd, dd MMMM yyyy")));

            CreateMap<Employees, EmployeeGridDto>()
                .ForMember(dest => dest.BirthdateString, opt => opt.MapFrom(src => src.BirthDate.Value.ToString("dddd, dd MMMM yyyy")));

        }
    }
}
