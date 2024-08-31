using FirebaseAdminAuthentication.DependencyInjection.Models;
using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Services.Courses;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using System.Security.Claims;

namespace GraphQLDemo.API.Schemas
{
    public class Mutation
    {
        private readonly CoursesRepository _coursesRepository;

        public Mutation(CoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }

        [Authorize(Policy ="IsAdmin")]
        public async Task<CourseResult> CreateCourse(
            string name, 
            Subject subject, 
            [Service] ITopicEventSender topicEventSender,
            ClaimsPrincipal claimsPrincipal)
        {
            string userId = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.ID);
            string email = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL);
            string username = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.USERNAME);
            string verifired = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL_VERIFIED);

            var currentCourseDTO = _coursesRepository.GetALL().Result.Where(i=>i.CreatorId== userId).FirstOrDefault();

            if (currentCourseDTO == null)
            {
                throw new GraphQLException(new Error("You do not have permition", "403"));
            }


            else
            {
                CourseDTO courseDTO = new CourseDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    CreatorId = userId,
                    Subject = subject,
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

        public async Task<CourseResult> UpdateCourse(Guid id, string name, Subject subject, [Service] ITopicEventSender topicEventSender)
        {
 
            var courseDTO = new CourseDTO
            {
                Id = id,
                Name = name,
                Subject = subject

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
