using EntityPracticeApp.Models;
using EntityPracticeApp.ViewModel;

namespace EntityPracticeApp.Service.Interface
{
    public interface IStudentCourseService
    {
        //List<CourseViewModel> SelectCourse(int studentId, string Name);
        List<CourseViewModel> SelectCourse(int studentId);
        List<StudentViewModel> SelectStudent(int cid);
        List<StudentViewModel> SelectStudent(int courseId, out List<StudentCourse> sc);

        List<CourseViewModel> MapCourse(int studentId);
        void PostMapCourses(int studentId, List<int> selectedCourses);
        List<StudentViewModel> MapStudent(int courseId);
        void MapStudentPost(int courseId, List<int> selectedStudents);
        void UpdateSingleStudent(StudentCourseViewModel studentCourseViewModel);
    }
}
