namespace GraphQLDemo.API.Models
{
    public class StudentType
    {
        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [GraphQLName("gpa")]
        public double GPA { get; set; }
    }
}
