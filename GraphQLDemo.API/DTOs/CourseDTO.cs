using GraphQLDemo.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphQLDemo.API.DTOs
{
    public class CourseDTO
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

        public Subject Subject { get; set; }

        public string InstructorId { get; set; }

        [ForeignKey(nameof(InstructorId))]
        public InstructorDTO Instructor { get; set; }

        public IEnumerable<StudentDTO> Students { get; set; }
    }

    public class InstructorDTO
    {
        [Key]
        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public double Salary { get; set; }

        public IEnumerable<CourseDTO> Courses { get; set; }
    }

    public class StudentDTO
    {
        [Key]
        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public double GPA { get; set; }

        public IEnumerable<CourseDTO>  Courses { get; set; }
    }
}
