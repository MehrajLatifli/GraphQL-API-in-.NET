using GraphQLDemo.API.DataLoaders;
using GraphQLDemo.API.Services.Instructors;

namespace GraphQLDemo.API.Models
{
    public class CourseType: IInterfaceResultSearchType,IUnionResultSearchType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public UserType Creator { get; private set; }
        public Subject Subject { get; set; }
        [IsProjected(true)]
        public Guid InstructorId { get; set; }
        public IEnumerable<StudentType> Students { get; set; }

        // Constructor to initialize CourseType with Creator
        public CourseType(string id, string name, UserType creator, Subject subject, Guid instructorId)
        {
            Id = id;
            Name = name;
            Creator = creator ?? throw new ArgumentNullException(nameof(creator));
            Subject = subject;
            InstructorId = instructorId;
        }

        [GraphQLNonNullType]
        public async Task<InstructorType> Instructor([Service] InstructorRepository instructorRepository)
        {
            var instructor = await instructorRepository.GetById(InstructorId);
            return new InstructorType
            {
                Id = instructor.Id.ToString(),
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                Salary = instructor.Salary
            };
        }

        [GraphQLNonNullType]
        public async Task<InstructorType> Instructor2([Service] InstructorDataLoader instructorDataLoader)
        {
            var instructor = await instructorDataLoader.LoadAsync(InstructorId, CancellationToken.None);
            return new InstructorType
            {
                Id = instructor.Id.ToString(),
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                Salary = instructor.Salary
            };
        }

        public string Description()
        {
            return $"Course: {Name}, {Subject}";
        }
    }
}
