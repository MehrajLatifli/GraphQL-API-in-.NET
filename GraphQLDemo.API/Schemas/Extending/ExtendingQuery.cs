using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Schemas.Filters;
using GraphQLDemo.API.Schemas.Sorters;
using GraphQLDemo.API.Services;
using GraphQLDemo.API.Services.Courses;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.API.Schemas
{
    [ExtendObjectType(typeof(Query))]
    public class ExtendingQuery
    {
        private readonly CoursesRepository _coursesRepository;

        public ExtendingQuery(CoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }



        [UseDbContext(typeof(SchoolDbContext))]
        public async Task<IEnumerable<IInterfaceResultSearchType>> InterfaceSearch(string item, [ScopedService] SchoolDbContext schoolDbContext)
        {

            var courseTypesTask = schoolDbContext.Courses
                .Where(c => c.Name.Contains(item))
                .Select(c => new CourseType(
                    c.Id.ToString(),
                    c.Name,
                    new UserType
                    {
                        Id = c.CreatorId.ToString(),
                        Username = c.Name
                    },
                    c.Subject,
                    c.InstructorId
                )).ToListAsync();

            var instructorTypesTask = schoolDbContext.Instructors
                .Where(c => c.FirstName.Contains(item) || c.LastName.Contains(item))
                .Select(c => new InstructorType
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Id = c.Id.ToString(),
                    Salary = c.Salary
                }).ToListAsync();


            var courseTypes = await courseTypesTask;
            var instructorTypes = await instructorTypesTask;


            return courseTypes.Cast<IInterfaceResultSearchType>().Concat(instructorTypes.Cast<IInterfaceResultSearchType>());
        }


        [UseDbContext(typeof(SchoolDbContext))]
        public async Task<IEnumerable<IInterfaceResultSearchType>> UnionSearch(string item, [ScopedService] SchoolDbContext schoolDbContext)
        {

            var courseTypesTask = schoolDbContext.Courses
                .Where(c => c.Name.Contains(item))
                .Select(c => new CourseType(
                    c.Id.ToString(),
                    c.Name,
                    new UserType
                    {
                        Id = c.CreatorId.ToString(),
                        Username = c.Name
                    },
                    c.Subject,
                    c.InstructorId
                )).ToListAsync();

            var instructorTypesTask = schoolDbContext.Instructors
                .Where(c => c.FirstName.Contains(item) || c.LastName.Contains(item))
                .Select(c => new InstructorType
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Id = c.Id.ToString(),
                    Salary = c.Salary
                }).ToListAsync();


            var courseTypes = await courseTypesTask;
            var instructorTypes = await instructorTypesTask;


            return courseTypes.Cast<IInterfaceResultSearchType>().Concat(instructorTypes.Cast<IInterfaceResultSearchType>());
        }


    }
}
