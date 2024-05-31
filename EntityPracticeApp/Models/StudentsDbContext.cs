using EntityPracticeApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityPracticeApp.Models
{
    public class DBContext:IdentityDbContext
    {
        public DBContext(DbContextOptions<DBContext> options ):base(options) 
        { 
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Define composite key for StudentCourse join table
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentsSid, sc.CoursesCourseId });
            modelBuilder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        }
    }

    internal class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.FirstName).HasMaxLength(255);
            builder.Property(x => x.LastName).HasMaxLength(255);
        }
    }
}
