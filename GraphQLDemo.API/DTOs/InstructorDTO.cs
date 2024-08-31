using System.ComponentModel.DataAnnotations;

namespace GraphQLDemo.API.DTOs
{
    public class InstructorDTO
    {
        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public double Salary { get; set; }

        public IEnumerable<CourseDTO> Courses { get; set; }
    }
}
