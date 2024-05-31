using EntityPracticeApp.Models;
using EntityPracticeApp.Service.Interface;
using EntityPracticeApp.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace EntityPracticeApp.Service
{
    public class StudentService : IStudentService
    {
        private readonly DBContext _dbContext;
        public StudentService(DBContext context)
        {
            _dbContext = context;
        }
        public void AddStudent(StudentViewModel svm)
        {
            var existingStudent = _dbContext.Students.FirstOrDefault(x => x.Email == svm.Email);
            if (existingStudent != null)
            {
                // If a student with the same email exists, throw an exception
                throw new InvalidOperationException("Email already exists");
            }
            else
            {
                // If no student with the same email exists, add the new student
                Student newStudent = new Student
                {
                    Name = svm.Name,
                    Email = svm.Email,
                    Phone = svm.Phone,
                    DOB = Convert.ToDateTime(svm.DateOfBirth),
                    Created = DateTime.Now,
                    Gender = svm.Gender == "Male" ? 1 : 0,
                };
                _dbContext.Students.Add(newStudent);
                _dbContext.SaveChanges();
            }
        }

        public void DeleteStudent(int id)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.Sid == id);
            _dbContext.Remove(student);
            _dbContext.SaveChanges();

        }

        public async Task <List<StudentViewModel>> GetAllStudent()
        {
            var stds = await _dbContext.Students.ToListAsync();
            List<StudentViewModel> studentViewModels = stds.Select(s => new StudentViewModel
            {
                Sid = s.Sid,
                Name = s.Name,
                Email = s.Email,
                Phone = s.Phone,
                DateOfBirth = s.DOB.ToString("dd MMM yyyy"),
                Gender = s.Gender == 1 ? "Male" : "Female",
            }).ToList();
            //foreach (var student in studentViewModels) {
            //    student.Sid = student.Sid;
            //}
            return studentViewModels;

        }

        public StudentViewModel GetStudent(StudentViewModel svm)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.Sid == svm.Sid);

                StudentViewModel svmViewModel = new StudentViewModel();
                svmViewModel.Sid = student.Sid;
                svmViewModel.Name = student.Name;
                svmViewModel.Email = student.Email;
                svmViewModel.Phone = student.Phone;
                svmViewModel.DateOfBirth = student.DOB.ToString("dd MMM yyyy");
                svmViewModel.Gender = student.Gender == 1 ? "Male" : "Female";
                return svmViewModel;

        }

        public  void UpdateStudent(StudentViewModel svm)
        {
            var student =  _dbContext.Students.FirstOrDefault(x => x.Sid == svm.Sid);

            student.Name = svm.Name;
            student.Phone = svm.Phone;
            student.Email = svm.Email;
            student.Updated = DateTime.Now;
            _dbContext.Students.Update(student);
            //_dbContext.Entry(student).CurrentValues.SetValues(svm);
             _dbContext.SaveChangesAsync();


        }
    }
}
