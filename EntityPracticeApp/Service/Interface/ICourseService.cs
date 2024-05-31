using EntityPracticeApp.ViewModel;

namespace EntityPracticeApp.Service.Interface
{
    public interface ICourseService
    {
        void AddCourse(CourseViewModel cvm);
        void RemoveCourse(int id);
        CourseViewModel GetCourse(CourseViewModel cvm);
        Task<List<CourseViewModel>> GetAllCourses();

        void UpdateCourse(CourseViewModel cvm);

    }
}
