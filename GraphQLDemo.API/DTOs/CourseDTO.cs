using GraphQLDemo.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphQLDemo.API.DTOs
{
    public class CourseDTO
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Subject Subject { get; set; }

        public string CreatorId { get; set; }

        public Guid InstructorId { get; set; }

        [ForeignKey(nameof(InstructorId))]
        public InstructorDTO Instructor { get; set; }

        public IEnumerable<StudentDTO> Students { get; set; }
    }
}
