using GraphQLDemo.API.Models;

namespace GraphQLDemo.API.Schemas
{
    public class Query
    {

        private readonly List<CourseType> _courses;
        public Query()
        {
            _courses = new List<CourseType>
            {
                new CourseType
                {
                    Id = Guid.NewGuid(),
                    Name = "Geometry",
                    Subject = Subject.Mathematics,

                    Instructor = new InstructorType
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Instructor1_FirstName",
                        LastName = "Instructor1_LastName",
                        Salary = 1000
                    },

                    Students = new List<StudentType>
                    {
                        new StudentType
                        {
                            Id = Guid.NewGuid(),
                            FirstName = "Student1_FirstName",
                            LastName = "Student1_LastName",
                            GPA = 100.0D
                        },
                        new StudentType
                        {
                            Id = Guid.NewGuid(),
                            FirstName = "Student2_FirstName",
                            LastName = "Student2_LastName",
                            GPA = 100.0D
                        }
                    }
                }
            };
        }

        public async Task <IEnumerable<CourseType>> GetCourses()
        {
            await Task.Delay(1000);
            return _courses;
        }

        public async Task <CourseType> GetCourse(Guid id)
        {
            await Task.Delay(1000);
            return _courses.FirstOrDefault(course => course.Id == id);
        }

        [GraphQLDeprecated("This query is depricated.")]
        public string Instruction => "hELLO graphQL";
    }
}
