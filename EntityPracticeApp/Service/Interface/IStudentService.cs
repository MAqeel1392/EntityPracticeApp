using EntityPracticeApp.ViewModel;

namespace EntityPracticeApp.Service.Interface
{
    public interface IStudentService
    {
        void AddStudent(StudentViewModel svm);

        void UpdateStudent(StudentViewModel svm);
        void DeleteStudent(int id);
        StudentViewModel GetStudent(StudentViewModel svm);
        Task<List<StudentViewModel>> GetAllStudent();
        

    }
}
