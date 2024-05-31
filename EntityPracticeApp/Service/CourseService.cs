using EntityPracticeApp.Service.Interface;
using EntityPracticeApp.ViewModel;
using Microsoft.EntityFrameworkCore;
using EntityPracticeApp.Models;

namespace EntityPracticeApp.Service
{
    public class CourseService : ICourseService
    {
        private readonly DBContext _dbContext;
        public CourseService(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddCourse(CourseViewModel cvm)
        {
            var existingCourse = _dbContext.Courses.FirstOrDefault(x=>x.Title == cvm.Title);

            if (existingCourse != null)
            {
                // If a student with the same email exists, throw an exception
                throw new InvalidOperationException("Course Title already exists");
            }
            else
            {
            Course newCourse = new Course()
            {
                Title = cvm.Title,
                Description = cvm.Description,
                CreatedDate = DateTime.Now,
            };
            _dbContext.Courses.Add(newCourse);
            _dbContext.SaveChanges();
            }
        }
        
        public async Task<List<CourseViewModel>> GetAllCourses()
        {
            var course = await _dbContext.Courses.ToListAsync();
            List<CourseViewModel> result = course.Select(x => new CourseViewModel
            {
                CourseId = x.CourseId,
                Title= x.Title,
                Description= x.Description,
            }).ToList();
            return result;
        }
        
        public CourseViewModel GetCourse(CourseViewModel cvm)
        {
            var course = _dbContext.Courses.FirstOrDefault(x=>x.CourseId == cvm.CourseId);
            cvm.Title = course.Title;
            cvm.Description = course.Description;
            return cvm;
        }
        
        public void RemoveCourse(int id)
        {
            var course = _dbContext.Courses.FirstOrDefault(x=>x.CourseId == id);
            _dbContext.Remove(course);
            _dbContext.SaveChanges();
        }
        
        public void UpdateCourse(CourseViewModel cvm)
        {
            var course = _dbContext.Courses.FirstOrDefault(x=>x.CourseId== cvm.CourseId);
            course.Title = cvm.Title;
            course.Description = cvm.Description;

            _dbContext.Courses.Update(course);
            _dbContext.SaveChanges();
        }
    }
}
