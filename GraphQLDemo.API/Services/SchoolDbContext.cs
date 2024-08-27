using GraphQLDemo.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.API.Services
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options)
        {
        }

        public DbSet<CourseDTO> Courses { get; set; }
        public DbSet<InstructorDTO> Instructors { get; set; }
        public DbSet<StudentDTO> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure CourseDTO
            modelBuilder.Entity<CourseDTO>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<CourseDTO>()
                .HasOne(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CourseDTO>()
                .HasMany(c => c.Students)
                .WithMany(s => s.Courses);

            // Configure InstructorDTO
            modelBuilder.Entity<InstructorDTO>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<InstructorDTO>()
                .HasMany(i => i.Courses)
                .WithOne(c => c.Instructor)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure StudentDTO
            modelBuilder.Entity<StudentDTO>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<StudentDTO>()
                .HasMany(s => s.Courses)
                .WithMany(c => c.Students);
        }
    }
}
