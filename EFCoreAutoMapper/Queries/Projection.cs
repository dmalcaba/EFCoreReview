using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCoreAutoMapper.Dtos;
using EFCoreReview.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreAutoMapper.Queries
{
    public class Projection
    {
        private readonly NorthwindContext _context;
        private readonly IMapper _mapper;

        public Projection(IMapper mapper)
        {
            _context = new NorthwindContext();
            _mapper = mapper;
        }

        /// <summary>
        /// This example how AutoMapper maps Employees to EmployeeDto based on convention.
        /// No configuration is needed.
        /// </summary>
        public async Task AutoProjectionAsync()
        {
            var query = _context.Employees.ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider);

            var result = await query.ToListAsync();

            /*
            info: Microsoft.EntityFrameworkCore.Database.Command[20101]
                  Executed DbCommand (93ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                  SELECT [dtoEmployees].[City], [dtoEmployees].[EmployeeID], [dtoEmployees].[FirstName], [dtoEmployees].[LastName]
                  FROM [Employees] AS [dtoEmployees]
             */
        }
    }
}
