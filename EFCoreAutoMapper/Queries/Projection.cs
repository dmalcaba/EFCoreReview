using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCoreAutoMapper.Dtos;
using EFCoreReview.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task ProjectAfterAsync()
        {
            var query = _context.Employees.Where(x => x.BirthDate < new DateTime(1952, 1, 1)).ProjectTo<EmployeeMappedDto>(_mapper.ConfigurationProvider);

            var result = await query.ToListAsync();
        }

        public async Task ProjectAfterV2Async()
        {
            var query = _context.Employees.Where(x => x.BirthDate < new DateTime(1952, 1, 1));

            var result = await query.ToListAsync();

            var project = _mapper.Map<List<Employees>, List<EmployeeMappedDto>>(result);
        }

        public async Task ProjectBeforeAsync()
        {
            var query = _context.Employees.ProjectTo<EmployeeMappedDto>(_mapper.ConfigurationProvider).Where(x => x.BirthDate.Value < new DateTime(1952, 1, 1));

            var result = await query.ToListAsync();
        }

        public async Task ProjectionExample()
        {
            var query = _context.Employees.AsQueryable()
                .Where(x => x.EmployeeId > 3 && x.Region == "WA")
                .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider);

            var result = await query.ToListAsync();
        }
    }
}
