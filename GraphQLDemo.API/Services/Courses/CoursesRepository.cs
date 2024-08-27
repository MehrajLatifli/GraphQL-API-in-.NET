using GraphQLDemo.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.API.Services.Courses
{
    public class CoursesRepository
    {
        private readonly IDbContextFactory<SchoolDbContext> _dbContextFactory;

        public CoursesRepository(IDbContextFactory<SchoolDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }


        public async Task<IEnumerable<CourseDTO>> GetALL()
        {
            try
            {
                using (SchoolDbContext context = await _dbContextFactory.CreateDbContextAsync())
                {
                 

                    return await context.Courses
                        .Include(c => c.Instructor)
                        .Include(c => c.Students).ToListAsync();

                }
            }
            catch (DbUpdateException ex)
            {

                throw new GraphQLException(new Error(message: ex.Message, code: "501"));
            }
        }

        public async Task<CourseDTO> GetById(Guid id)
        {
            try
            {
                using (SchoolDbContext context = await _dbContextFactory.CreateDbContextAsync())
                {


                    return await context.Courses
                         .Include(c => c.Instructor)
                        .Include(c => c.Students).FirstOrDefaultAsync(c=>c.Id==id.ToString());

                }
            }
            catch (DbUpdateException ex)
            {

                throw new GraphQLException(new Error(message: ex.Message, code: "501"));
            }
        }


        public async Task<CourseDTO> Create(CourseDTO courseDTO)
        {
            try
            {
                using (SchoolDbContext context = await _dbContextFactory.CreateDbContextAsync())
                {
                    context.Courses.Add(courseDTO);
                    await context.SaveChangesAsync();

                    return courseDTO;

                }
            }
            catch (DbUpdateException ex)
            {

                throw new GraphQLException(new Error(message: ex.Message, code: "501"));
            }
        }

        public async Task<CourseDTO> Update(CourseDTO courseDTO)
        {
            try
            {
                using (var context = await _dbContextFactory.CreateDbContextAsync())
                {
 
                    var existingCourse = await context.Courses
                        .Include(c => c.Instructor)
                        .Include(c => c.Students)
                        .FirstOrDefaultAsync(c => c.Id == courseDTO.Id);

                    if (existingCourse == null)
                    {
                        throw new GraphQLException(new Error(message: "Course not found.", code: "404"));
                    }

                
                    existingCourse.Name = courseDTO.Name;
                    existingCourse.Subject = courseDTO.Subject;

                    if (courseDTO.Instructor != null)
                    {
                        var instructor = await context.Instructors.FindAsync(courseDTO.Instructor.Id);
                        if (instructor != null)
                        {
                            instructor.FirstName = courseDTO.Instructor.FirstName;
                            instructor.LastName = courseDTO.Instructor.LastName;
                            instructor.Salary = courseDTO.Instructor.Salary;
                        }
                        else
                        {
                           
                            context.Instructors.Add(courseDTO.Instructor);
                        }
                    }

                    if (courseDTO.Students != null)
                    {
                        var existingStudentIds = existingCourse.Students.Select(s => s.Id).ToList();
                        var updatedStudentIds = courseDTO.Students.Select(s => s.Id).ToList();

          
                        var studentsToRemove = existingCourse.Students.Where(s => !updatedStudentIds.Contains(s.Id)).ToList();
                        context.Students.RemoveRange(studentsToRemove);

                        foreach (var student in courseDTO.Students)
                        {
                            var existingStudent = await context.Students.FindAsync(student.Id);
                            if (existingStudent != null)
                            {
                                existingStudent.FirstName = student.FirstName;
                                existingStudent.LastName = student.LastName;
                                existingStudent.GPA = student.GPA;
                            }
                            else
                            {
                                context.Students.Add(student);
                            }
                        }
                    }

                    context.Courses.Update(existingCourse);
                    await context.SaveChangesAsync();
                    return existingCourse;
                }
            }
            catch (DbUpdateException ex)
            {
                throw new GraphQLException(new Error(message: ex.Message, code: "501"));
            }
        }



        public async Task<bool> Delete(Guid id)
        {
            using (var context = await _dbContextFactory.CreateDbContextAsync())
            {
            
                var courseId = id.ToString();
                var course = await context.Courses.FindAsync(courseId);

                if (course == null)
                {
                    throw new GraphQLException(new Error(message: "Course not found.", code: "404"));
                }

                context.Courses.Remove(course);
                var result = await context.SaveChangesAsync();

                return result > 0;
            }
        }



    }
}
