using GraphQLDemo.API.Models;

namespace GraphQLDemo.API.Schemas
{
    public class Mutation
    {
        private readonly List<CourseResult> _courses;

        public Mutation(List<CourseResult> courses)
        {
            _courses = courses ?? new List<CourseResult>();

        }


        public CourseResult CreateCourse(string name, Subject subject, Guid instructorId, IEnumerable<Guid> studentIds)
        {

            var course = new CourseResult
            {
                Id = Guid.NewGuid(),
                Name = name,
                Subject = subject,
                Instructor = new InstructorType()
                {
                    Id = instructorId,
                    FirstName = "Instructor_FirstName",
                    LastName = "Instructor_LastName",
                    Salary = 10000
                },
                Students = new List<StudentType>()
                {
                    new StudentType()
                    {
                        Id=studentIds.FirstOrDefault(),
                        FirstName = "Student_FirstName",
                        LastName = "Student_LastName",
                        GPA=100.0
                    },
                    new StudentType()
                    {
                        Id=studentIds.LastOrDefault(),
                        FirstName = "Student_FirstName",
                        LastName = "Student_LastName",
                        GPA=100.0
                    }
                }
            };

            _courses.Add(course);

            return course;
        }


        public CourseResult UpdateCourse(Guid Id, string name, Subject subject, Guid instructorId, IEnumerable<Guid> studentIds)
        {

            CourseResult course = _courses.FirstOrDefault(c => c.Id == Id);

            if (course == null)
            {
                throw new GraphQLException(new Error(message: "Cource not found.", code: "404"));
                //throw new Exception("Course not fount");
            }


            course.Name = name;
            course.Subject = subject;
            course.Instructor = new InstructorType()
            {
                Id = instructorId,
                FirstName = "Instructor_FirstNameUpdated",
                LastName = "Instructor_LastNameUpdated",
                Salary = 10000
            };
            course.Students = new List<StudentType>()
            {
                new StudentType()
                {
                    Id=studentIds.FirstOrDefault(),
                    FirstName = "Student_FirstNameUpdated",
                    LastName = "Student_LastNameUpdated",
                    GPA=100.0
                },
                new StudentType()
                {
                    Id=studentIds.LastOrDefault(),
                    FirstName = "Student_FirstNameUpdated",
                    LastName = "Student_LastNameUpdated",
                    GPA=100.0
                }
            };



            return course;
        }

        public List<CourseResult> DeleteCourse(Guid Id)
        {
            if (_courses.Where(i => i.Id == Id).Any())
            {



                _courses.RemoveAll(c => c.Id == Id);
                return _courses;
            }
            else
            {
                throw new GraphQLException(new Error(message: "Cource not found.", code: "404"));
            }
        }
    }
}
