using EntityPracticeApp.Models;
using EntityPracticeApp.Service.Interface;
using EntityPracticeApp.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace EntityPracticeApp.Service
{
    public class StudentCourseService : IStudentCourseService
    {
        private readonly DBContext _dbContext;
        public StudentCourseService(DBContext dbContext) {
            _dbContext = dbContext;
        }
        public List<CourseViewModel> 
            SelectCourse(int studentId)
        {
            var query = from s in _dbContext.Students
                        join cs in _dbContext.StudentCourses on s.Sid equals cs.StudentsSid
                        join c in _dbContext.Courses on cs.CoursesCourseId equals c.CourseId
                        where s.Sid == studentId
                        select new
                        {
                            //Student = s,
                            Course = c,
                        };

            var result = query.ToList();
            List<CourseViewModel> cvmList = new List<CourseViewModel>();
            foreach (var item in result)
            {
                CourseViewModel cvm = new CourseViewModel();
                cvm.CourseId = item.Course.CourseId;
                cvm.Title = item.Course.Title;
                cvm.Description = item.Course.Description;
                cvmList.Add(cvm);
            }
            return cvmList;
        }
        public List<StudentViewModel> SelectStudent(int courseId)
        {
            var studentcourse = _dbContext.StudentCourses.Where(x=>x.CoursesCourseId == courseId && x.CourseAssigned==true).ToList();
            List<StudentViewModel> students = new List<StudentViewModel>();

            foreach (var student in studentcourse)
            {
                StudentViewModel studentViewModel = new StudentViewModel();
                studentViewModel.Sid = student.StudentsSid;
                students.Add(studentViewModel);
            }
            return students;

        }
        public List<StudentViewModel> SelectStudent(int courseId,out List<StudentCourse> sc)
        {
            var query = from s in _dbContext.Students
                        join cs in _dbContext.StudentCourses on s.Sid equals cs.StudentsSid
                        join c in _dbContext.Courses on cs.CoursesCourseId equals c.CourseId
                        where c.CourseId == courseId
                        select new
                        {
                            Student = s,
                            StudentCourse = cs,
                            //Course = c,
                        };

            var result = query.ToList();
            List<StudentCourse> svmList1 = new List<StudentCourse>();

            foreach (var item in result)
            {
                svmList1.Add(item.StudentCourse);
            }
            sc = svmList1;
            // List<StudentViewModel> svmList = new List<StudentViewModel>();
            List<StudentViewModel> svmList = new List<StudentViewModel>();

            foreach (var item in result)
            {
                StudentViewModel svm = new StudentViewModel();
                svm.Sid = item.Student.Sid;
                svm.Name = item.Student.Name;
                svm.Email = item.Student.Email;
                svm.Phone = item.Student.Phone;
                svm.Gender = item.Student.Gender.ToString();
                svmList.Add(svm);
            }
            return svmList;
        }

        public void PostMapCourses(int studentId, List<int> selectedCourses)
        {
            var student = _dbContext.StudentCourses.Where(x => x.StudentsSid == studentId).ToList();
            foreach(var st in student) {
                _dbContext.Remove(st);
            }         
            _dbContext.SaveChanges();
            foreach (int item in selectedCourses)
            {
                StudentCourse sc = new StudentCourse();
                sc.CoursesCourseId = item;
                sc.StudentsSid = studentId;
                _dbContext.StudentCourses.Add(sc);
            }
            _dbContext.SaveChanges();
        }

        public List<CourseViewModel> MapCourse(int studentId)
        {
            var query = from c in _dbContext.Courses
                            //join cs in _dbContext.CourseStudent on s.Sid equals cs.StudentsSid
                            //join c in _dbContext.Courses on cs.CoursesCourseId equals c.CourseId
                        select new
                        {
                            //Student = s,
                            Course = c, 
                        };

            var result = query.ToList();
            List<CourseViewModel> cvmList = new List<CourseViewModel>();
            foreach (var item in result)
            {
                CourseViewModel cvm = new CourseViewModel();
                cvm.CourseId = item.Course.CourseId;
                cvm.Title = item.Course.Title;
                cvm.Description = item.Course.Description;
                cvmList.Add(cvm);
            }

            List<CourseViewModel> cvmListSelected = SelectCourse(studentId);
            foreach (var item in cvmListSelected)
            {
                foreach (var item1 in cvmList)
                {
                    if (item.CourseId == item1.CourseId)
                    {
                        item1.isAssigned = true;
                    }
                }
            }

            return cvmList;
        }

        public List<StudentViewModel> MapStudent(int courseId)
        {
            var query = from s in _dbContext.Students
                            //join cs in _dbContext.CourseStudent on s.Sid equals cs.StudentsSid
                            //join c in _dbContext.Courses on cs.CoursesCourseId equals c.CourseId
                        select new
                        {
                            Student = s,
                            //Course = c,
                        };

            var result = query.ToList();
            List<StudentViewModel> svmList = new List<StudentViewModel>();
            foreach (var item in result)
            {
                StudentViewModel svm = new StudentViewModel();
                svm.Sid = item.Student.Sid;
                svm.Name = item.Student.Name;
                svm.Email = item.Student.Email;
                svm.Phone = item.Student.Phone;
                svm.DateOfBirth = item.Student.DOB.ToString("dd MMM yyyy");
                svm.Gender = item.Student.Gender == 1 ? "Male" : "Female";
                svmList.Add(svm);
            }

            List<StudentViewModel> svmListSelected = SelectStudent(courseId,out List<StudentCourse> scList);
            
            //foreach (var item in svmListSelected)
            //{
                foreach (var item1 in svmList)
                {
                    //if (item.Sid == item1.Sid && scList)
                    //{
                    //    item1.isSelected = true;
                    //}

                foreach (var item2 in scList)
                {
                    if (item1.Sid == item2.StudentsSid && courseId == item2.CoursesCourseId && item2.CourseAssigned == true)
                    {
                        item1.isAssigned = true;
                    }
                }
                }
            //}

            return svmList;

        }
        public void MapStudentPost(int courseId, List<int> studentList)
        {
            // Fetch all existing mappings for the given courseId
            var studentCourses = _dbContext.StudentCourses.Where(x => x.CoursesCourseId == courseId).ToList();
            var courses = _dbContext.Courses.Where(x => x.CourseId == courseId).ToList();

            if (studentList != null && studentList.Any())
            {
                // Add new mappings if studentList is not empty
                foreach (var student in studentList)
                {
                    // Check if the mapping already exists
                    var existingMapping = studentCourses.FirstOrDefault(sc => sc.StudentsSid == student);
                    if (existingMapping == null)
                    {
                        // Add new mapping
                        var newMapping = new StudentCourse
                        {
                            CoursesCourseId = courseId,
                            StudentsSid = student,
                            CourseAssigned = true,
                            Created = DateTime.Now,
                            LastUpdated = DateTime.Now
                        };
                        _dbContext.StudentCourses.Add(newMapping);
                    }
                    else
                    {
                        // Update existing mapping
                        existingMapping.CourseAssigned = true;
                        existingMapping.LastUpdated = DateTime.Now;
                        _dbContext.StudentCourses.Update(existingMapping);
                    }
                }
            }
            else
            {
                // If studentList is empty, update existing mappings
                foreach (var mapping in studentCourses)
                {
                    mapping.CourseAssigned = false;
                    mapping.LastUpdated = DateTime.Now;
                    _dbContext.StudentCourses.Update(mapping);
                }
            }

            // Update isAssigned flag for courses
            _dbContext.Courses.UpdateRange(courses);

            _dbContext.SaveChanges();
        }


        public void UpdateSingleStudent(StudentCourseViewModel studentCourseViewModel)
        {
            var data =  _dbContext.StudentCourses
                .FirstOrDefault(x => x.StudentsSid == studentCourseViewModel.StudentsSid && x.CoursesCourseId == studentCourseViewModel.CoursesCourseId);

            Console.WriteLine("Data: "+data);
            if (data != null)
            {
                data.LastUpdated = DateTime.Now;
                data.CourseAssigned = studentCourseViewModel.CourseAssigned;
                _dbContext.StudentCourses.Remove(data);
            }
            else
            {
                StudentCourse scvm = new StudentCourse
                {
                    StudentsSid = studentCourseViewModel.StudentsSid,
                    CoursesCourseId = studentCourseViewModel.CoursesCourseId,
                    Created = DateTime.Now,
                    CourseAssigned = studentCourseViewModel.CourseAssigned
                };

                _dbContext.StudentCourses.Add(scvm);
            }
             _dbContext.SaveChanges();
        }
    }
}
    

