using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Schemas.Filters;
using GraphQLDemo.API.Services;
using GraphQLDemo.API.Services.Courses;

namespace GraphQLDemo.API.Schemas
{
    public class Query
    {

        private readonly CoursesRepository _coursesRepository;
        public Query(CoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }


        public async Task <IEnumerable<CourseType>> GetCourses()
        {
            await Task.Delay(1000);

            IEnumerable<CourseDTO> courseDTOs = await _coursesRepository.GetALL();

           var coursetypes_ = courseDTOs.Select(c => new CourseType()
            {
                Id = c.Id.ToString(),
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,
               //Instructor = new InstructorType()
               //{
               //    Id = c.Instructor.Id,
               //    FirstName = c.Instructor.FirstName,
               //    LastName = c.Instructor.LastName,
               //    Salary = c.Instructor.Salary,

               //},
               //Students = c.Students?.Select(s => new StudentType
               //{
               //    Id = s.Id,
               //    FirstName = s.FirstName,
               //    LastName = s.LastName,
               //    GPA = s.GPA
               //}).ToList() ?? new List<StudentType>()
           });

            return coursetypes_;
        }

    
        public async Task <CourseType> GetCourse(Guid id)
        {
            await Task.Delay(1000);

            CourseDTO courseDTO = await _coursesRepository.GetById(id);

            var coursetype = new CourseType()
            {
                Id = courseDTO.Id.ToString(),
                Name = courseDTO.Name,
                Subject = courseDTO.Subject,
                InstructorId = courseDTO.InstructorId,
                //Instructor = new InstructorType()
                //{
                //    Id = courseDTO.Instructor.Id,
                //    FirstName = courseDTO.Instructor.FirstName,
                //    LastName = courseDTO.Instructor.LastName,
                //    Salary = courseDTO.Instructor.Salary,

                //},
                //Students = courseDTO.Students?.Select(s => new StudentType
                //{
                //    Id = s.Id,
                //    FirstName = s.FirstName,
                //    LastName = s.LastName,
                //    GPA = s.GPA
                //}).ToList() ?? new List<StudentType>()
            };

            return coursetype;


        }

        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 2)]
        public async Task<IEnumerable<CourseType>> GetCoursesPages()
        {
            await Task.Delay(1000);

            IEnumerable<CourseDTO> courseDTOs = await _coursesRepository.GetALL();

            var coursetypes_ = courseDTOs.Select(c => new CourseType()
            {
                Id = c.Id.ToString(),
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,
                //Instructor = new InstructorType()
                //{
                //    Id = c.Instructor.Id,
                //    FirstName = c.Instructor.FirstName,
                //    LastName = c.Instructor.LastName,
                //    Salary = c.Instructor.Salary,

                //},
                //Students = c.Students?.Select(s => new StudentType
                //{
                //    Id = s.Id,
                //    FirstName = s.FirstName,
                //    LastName = s.LastName,
                //    GPA = s.GPA
                //}).ToList() ?? new List<StudentType>()
            });

            return coursetypes_;
        }


        [UseDbContext(typeof(SchoolDbContext))]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 2)]
        [UseFiltering(typeof(CourseFilterType))]
        public async Task<IQueryable<CourseType>> GetCoursesPagesUseDbContext([ScopedService] SchoolDbContext schoolDbContext)
        {
            await Task.Delay(1000);

          

            var coursetypes_ = schoolDbContext.Courses.Select(c => new CourseType()
            {
                Id = c.Id.ToString(),
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,

            });

            return coursetypes_;
        }


        [UseOffsetPaging(IncludeTotalCount = true)]
        public async Task<IEnumerable<CourseType>> GetCoursesOffsetPaging()
        {
            await Task.Delay(1000);

            IEnumerable<CourseDTO> courseDTOs = await _coursesRepository.GetALL();

            var coursetypes_ = courseDTOs.Select(c => new CourseType()
            {
                Id = c.Id.ToString(),
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,
                //Instructor = new InstructorType()
                //{
                //    Id = c.Instructor.Id,
                //    FirstName = c.Instructor.FirstName,
                //    LastName = c.Instructor.LastName,
                //    Salary = c.Instructor.Salary,

                //},
                //Students = c.Students?.Select(s => new StudentType
                //{
                //    Id = s.Id,
                //    FirstName = s.FirstName,
                //    LastName = s.LastName,
                //    GPA = s.GPA
                //}).ToList() ?? new List<StudentType>()
            });

            return coursetypes_;
        }

        [GraphQLDeprecated("This query is depricated.")]
        public string Instruction => "hELLO graphQL";
    }
}
