using GraphQLDemo.API.DataLoaders;
using GraphQLDemo.API.Services.Instructors;

namespace GraphQLDemo.API.Models
{
    public class CourseType
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Subject Subject { get; set; }

        [GraphQLIgnore]
        public Guid InstructorId { get; set; }


        [GraphQLNonNullType]
        public async Task <InstructorType> Instructor([Service] InstructorRepository instructorRepository)
        {
            var i = await instructorRepository.GetById(InstructorId);

            return new InstructorType()
            {
                Id = i.Id,
                FirstName = i.FirstName,
                LastName = i.LastName,
                Salary = i.Salary,

            };
        }

        [GraphQLNonNullType]
        public async Task<InstructorType> Instructor2([Service] InstructorDataLoader  instructorDataLoader)
        {
            var i = await instructorDataLoader.LoadAsync(InstructorId, cancellationToken:CancellationToken.None);

            return new InstructorType()
            {
                Id = i.Id,
                FirstName = i.FirstName,
                LastName = i.LastName,
                Salary = i.Salary,

            };
        }

        public IEnumerable<StudentType> Students { get; set; }

        public string Description()
        {
  

            return $"Course: {Name}, {Subject}";
        }
    }
}
