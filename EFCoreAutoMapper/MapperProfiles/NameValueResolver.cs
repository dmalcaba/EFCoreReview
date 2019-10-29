using AutoMapper;
using EFCoreAutoMapper.Dtos;
using EFCoreReview.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreAutoMapper.MapperProfiles
{
    public class NameValueResolver : IValueResolver<Employees, EmployeeMappedDto, string>
    {
        public string Resolve(Employees source, EmployeeMappedDto destination, string destMember, ResolutionContext context)
        {
            return source.FirstName + " " + source.LastName;
        }
    }
}
