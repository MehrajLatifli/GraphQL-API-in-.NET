namespace GraphQLDemo.API.Models
{
    public class CourseResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CreatorId { get; set; }

        public Subject Subject { get; set; }

        [GraphQLNonNullType]
        public InstructorType Instructor { get; set; }

        public IEnumerable<StudentType> Students { get; set; }
    }
}
