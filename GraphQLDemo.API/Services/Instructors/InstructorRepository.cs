using GraphQLDemo.API.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Services.Instructors
{
    public class InstructorRepository
    {
        private readonly IDbContextFactory<SchoolDbContext> _dbContextFactory;

        public InstructorRepository(IDbContextFactory<SchoolDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<InstructorDTO>> GetALL()
        {
            try
            {
                using (var context = await _dbContextFactory.CreateDbContextAsync())
                {
                    return await context.Instructors.ToListAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new GraphQLException(new Error(message: ex.Message, code: "501"));
            }
        }

        public async Task<InstructorDTO> GetById(Guid id)
        {
            try
            {
                using (var context = await _dbContextFactory.CreateDbContextAsync())
                {
                    return await context.Instructors.FirstOrDefaultAsync(c => c.Id == id);
                }
            }
            catch (DbUpdateException ex)
            {
                throw new GraphQLException(new Error(message: ex.Message, code: "501"));
            }
        }

        public async Task<IEnumerable<InstructorDTO>> GetManyByIds(IReadOnlyList<Guid> guids)
        {
            try
            {
                using (var context = await _dbContextFactory.CreateDbContextAsync())
                {
                 

                    return await context.Instructors
                        .Where(i => guids.Contains(i.Id))
                        .ToListAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new GraphQLException(new Error(message: ex.Message, code: "501"));
            }
        }
    }
}
