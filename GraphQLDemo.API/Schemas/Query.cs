using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Schemas.Filters;
using GraphQLDemo.API.Schemas.Sorters;
using GraphQLDemo.API.Services;
using GraphQLDemo.API.Services.Courses;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.API.Schemas
{
    public class Query
    {
        private readonly CoursesRepository _coursesRepository;

        public Query(CoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }

        [UseSorting(typeof(CourseSortType))]
        public async Task<IEnumerable<CourseType>> GetCourses()
        {
            await Task.Delay(1000); // Simulate async operation

            var courseDTOs = await _coursesRepository.GetALL();
            var coursetypes = courseDTOs.Select(c => new CourseType(
                c.Id.ToString(),
                c.Name,
                new UserType
                {
                    Id = c.CreatorId.ToString(),
                    Username = c.Name
                },
                c.Subject,
                c.InstructorId
            )
            {
                Students = c.Students?.Select(s => new StudentType
                {
                    Id = s.Id.ToString(),
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    GPA = s.GPA
                }).ToList() ?? new List<StudentType>()
            });

            return coursetypes;
        }

        public async Task<CourseType> GetCourse(Guid id)
        {
            await Task.Delay(1000); // Simulate async operation

            var courseDTO = await _coursesRepository.GetById(id);
            var courseType = new CourseType(
                courseDTO.Id.ToString(),
                courseDTO.Name,
                new UserType
                {
                    Id = courseDTO.CreatorId.ToString(),
                    Username = courseDTO.Name
                },
                courseDTO.Subject,
                courseDTO.InstructorId
            )
            {
                Students = courseDTO.Students?.Select(s => new StudentType
                {
                    Id = s.Id.ToString(),
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    GPA = s.GPA
                }).ToList() ?? new List<StudentType>()
            };

            return courseType;
        }




        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 2)]
        public async Task<IEnumerable<CourseType>> GetCoursesPages()
        {
            await Task.Delay(1000); // Simulate async operation

            var courseDTOs = await _coursesRepository.GetALL();
            var coursetypes = courseDTOs.Select(c => new CourseType(
                c.Id.ToString(),
                c.Name,
                new UserType
                {
                    Id = c.CreatorId.ToString(),
                    Username = c.Name
                },
                c.Subject,
                c.InstructorId
            ));

            return coursetypes;
        }

        [UseDbContext(typeof(SchoolDbContext))]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 50)]
        [UseProjection]
        [UseFiltering(typeof(CourseFilterType))]
        [UseSorting]
        public async Task<IQueryable<CourseType>> GetCoursesPagesUseDbContext([ScopedService] SchoolDbContext schoolDbContext)
        {
            await Task.Delay(1000); // Simulate async operation

            var coursetypes = schoolDbContext.Courses.Select(c => new CourseType(
                c.Id.ToString(),
                c.Name,
                new UserType
                {
                    Id = c.CreatorId.ToString(),
                    Username = c.Name
                },
                c.Subject,
                c.InstructorId
            ));

            return coursetypes;
        }

        [UseOffsetPaging(IncludeTotalCount = true)]
        public async Task<IEnumerable<CourseType>> GetCoursesOffsetPaging()
        {
            await Task.Delay(1000); // Simulate async operation

            var courseDTOs = await _coursesRepository.GetALL();
            var coursetypes = courseDTOs.Select(c => new CourseType(
                c.Id.ToString(),
                c.Name,
                new UserType
                {
                    Id = c.CreatorId.ToString(),
                    Username = c.Name
                },
                c.Subject,
                c.InstructorId
            ));

            return coursetypes;
        }

        [GraphQLDeprecated("This query is deprecated.")]
        public string Instruction => "Hello GraphQL";
    }
}
