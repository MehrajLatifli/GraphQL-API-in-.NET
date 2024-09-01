using AppAny.HotChocolate.FluentValidation;
using FirebaseAdminAuthentication.DependencyInjection.Models;
using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Middlewares.UseUser;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Services.Courses;
using GraphQLDemo.API.Validators;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using System.Security.Claims;

namespace GraphQLDemo.API.Schemas
{
    public class Mutation
    {
        private readonly CoursesRepository _coursesRepository;
        private readonly CourseTypeInputValidation _validationRules;

        public Mutation(CoursesRepository coursesRepository, CourseTypeInputValidation validationRules)
        {
            _coursesRepository = coursesRepository;
            _validationRules = validationRules;
        }

        [Authorize(Policy ="IsAdmin")]
        [UseUser]
        public async Task<CourseResult> CreateCourse(CourseTypeInput courseTypeInput , [Service] ITopicEventSender topicEventSender, [User] User user)
        {
            await Validate(courseTypeInput);

            string userId = user.Id;
            string email = user.Email;
            string username = user.Username;
            bool verifired = user.EmailVerified;

            var currentCourseDTO = _coursesRepository.GetALL().Result.Where(i => i.CreatorId == userId).FirstOrDefault();

            if (currentCourseDTO == null)
            {
                throw new GraphQLException(new Error("You do not have permition", "403"));
            }


            else
            {
                CourseDTO courseDTO = new CourseDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = courseTypeInput.Name,
                    CreatorId = userId,
                    Subject = courseTypeInput.Subject,
                    Instructor = new InstructorDTO()
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Instructor_FirstName",
                        LastName = "Instructor_LastName",
                        Salary = 10000
                    },
                    Students = new List<StudentDTO>
                {
                    new StudentDTO
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Student_FirstName1",
                        LastName = "Student_LastName1",
                        GPA = 100.0
                    },

                    new StudentDTO
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Student_FirstName2",
                        LastName = "Student_LastName2",
                        GPA = 100.0
                    },

                }

                };

                var c = await _coursesRepository.Create(courseDTO);

                var course = new CourseResult
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Subject = c.Subject,
                    CreatorId = c.CreatorId,
                    Instructor = new InstructorType()
                    {
                        Id = c.InstructorId.ToString(),
                        FirstName = "Instructor_FirstName",
                        LastName = "Instructor_LastName",
                        Salary = 10000
                    },
                    Students = c.Students.Select(s => new StudentType
                    {
                        Id = s.Id.ToString(),
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        GPA = s.GPA
                    }).ToList()
                };


                await topicEventSender.SendAsync(nameof(Subscription.CourceCreate), course);

                return course;
            }
        }

        private async Task Validate(CourseTypeInput courseTypeInput)
        {
            var validationresult = await _validationRules.ValidateAsync(courseTypeInput);

            if (!validationresult.IsValid)
            {
                throw new GraphQLException(new Error("Invalid input", "422"));
            }
        }


        public async Task<CourseResult> UpdateCourse(Guid id, [UseFluentValidation, UseValidator<CourseTypeInputValidation>] CourseTypeInput courseTypeInput, [Service] ITopicEventSender topicEventSender)
        {
     

            var courseDTO = new CourseDTO
            {
                Id = id,
                Name = courseTypeInput.Name,
                Subject = courseTypeInput.Subject

            };

         
            var updatedCourse = await _coursesRepository.Update(courseDTO);

            if (updatedCourse == null)
            {
                throw new InvalidOperationException("Course update failed.");
            }

            
            var courseResult = new CourseResult
            {
                Id = updatedCourse.Id.ToString(),
                Name = updatedCourse.Name,
                Subject = updatedCourse.Subject,
                Instructor = updatedCourse.Instructor != null ? new InstructorType
                {
                    Id = updatedCourse.Instructor.Id.ToString(),
                    FirstName = updatedCourse.Instructor.FirstName ?? "Default_FirstName",
                    LastName = updatedCourse.Instructor.LastName ?? "Default_LastName",
                    Salary = updatedCourse.Instructor.Salary
                } : new InstructorType
                {
                    Id = string.Empty,
                    FirstName = "Default_FirstName",
                    LastName = "Default_LastName",
                    Salary = 0
                },
                Students = (updatedCourse.Students ?? new List<StudentDTO>()).Select(s => new StudentType
                {
                    Id = s.Id.ToString(),
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    GPA = s.GPA
                }).ToList()
            };

        
            string courseId = $"{courseResult.Id}_{nameof(Subscription.CourseUpdate)}";
            await topicEventSender.SendAsync(courseId, courseResult);

            return courseResult;
        }


        public async Task<bool> DeleteCourse(Guid Id, [Service] ITopicEventSender topicEventSender)
        {
  
            return await _coursesRepository.Delete(Id);
        }
    }
}
