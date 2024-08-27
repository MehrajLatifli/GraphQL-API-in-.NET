namespace GraphQLDemo.API.Models
{
    public class CourseType
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Subject Subject { get; set; }

        [GraphQLNonNullType]
        public InstructorType Instructor { get; set; }

        public IEnumerable<StudentType> Students { get; set; }

        public string Description()
        {
            var student = Students.Any()
         ? string.Join(", ", Students.Select(s => $"{s.FirstName}, {s.LastName}"))
         : "No students enrolled";

            return $"Course: {Name}, {Subject};  Instructor: {Instructor.FirstName}, {Instructor.LastName};  Student: {student}";
        }
    }
}
