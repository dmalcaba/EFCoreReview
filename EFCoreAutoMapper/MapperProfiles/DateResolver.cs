using AutoMapper;
using EFCoreAutoMapper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreAutoMapper.MapperProfiles
{
    public class DateResolver : IMemberValueResolver<object, object, DateTime?, Date>
    {
        public Date Resolve(object source, object destination, DateTime? sourceMember, Date destMember, ResolutionContext context)
        {
            return new Date(sourceMember);
        }
    }
}
