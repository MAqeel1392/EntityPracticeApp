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
        public List<CourseViewModel> SelectCourse(int studentId, out List<StudentCourse> sc)
        {
            var query = from s in _dbContext.Students
                        join cs in _dbContext.StudentCourses on s.Sid equals cs.StudentsSid
                        join c in _dbContext.Courses on cs.CoursesCourseId equals c.CourseId
                        where s.Sid == studentId && cs.CourseAssigned == true
                        select new
                        {
                            Course = c,
                            StudentCourse = cs
                        };

            var result = query.ToList();

            // Initialize the lists
            List<CourseViewModel> cvmList = new List<CourseViewModel>();
            List<StudentCourse> studentCoursesList = new List<StudentCourse>();

            foreach (var item in result)
            {
                // Populate CourseViewModel list
                CourseViewModel cvm = new CourseViewModel
                {
                    CourseId = item.Course.CourseId,
                    Title = item.Course.Title,
                    Description = item.Course.Description
                };
                cvmList.Add(cvm);

                // Populate StudentCourse list
                studentCoursesList.Add(item.StudentCourse);
            }

            sc = studentCoursesList;
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
        public List<StudentViewModel> SelectStudent(int courseId, out List<StudentCourse> sc)
        {
            var query = from s in _dbContext.Students
                        join cs in _dbContext.StudentCourses on s.Sid equals cs.StudentsSid
                        join c in _dbContext.Courses on cs.CoursesCourseId equals c.CourseId
                        where c.CourseId == courseId && cs.CourseAssigned == true
                        select new
                        {
                            Student = s,
                            StudentCourse = cs
                        };

            var result = query.ToList();

            // Initialize the lists
            List<StudentViewModel> svmList = new List<StudentViewModel>();
            List<StudentCourse> studentCoursesList = new List<StudentCourse>();

            foreach (var item in result)
            {
                // Populate StudentViewModel list
                StudentViewModel svm = new StudentViewModel
                {
                    Sid = item.Student.Sid,
                    Name = item.Student.Name,
                    Email = item.Student.Email,
                    Phone = item.Student.Phone,
                    Gender = item.Student.Gender.ToString()
                };
                svmList.Add(svm);

                // Populate StudentCourse list
                studentCoursesList.Add(item.StudentCourse);
            }

            sc = studentCoursesList;
            return svmList;
        }

        public void PostMapCourses(int studentId, List<int> selectedCourses)
        {
            // Fetch all existing mappings for the given studentId
            var existingMappings = _dbContext.StudentCourses.Where(sc => sc.StudentsSid == studentId).ToList();
            var student = _dbContext.Students.Where(s => s.Sid == studentId).FirstOrDefault();

            // Iterate over existing mappings
            foreach (var mapping in existingMappings)
            {
                if (selectedCourses.Contains(mapping.CoursesCourseId))
                {
                    // If the course is in the list, ensure CourseAssigned is true
                    mapping.CourseAssigned = true;
                    mapping.LastUpdated = DateTime.Now;
                    selectedCourses.Remove(mapping.CoursesCourseId); // Remove the course from the list as it's already mapped
                }
                else
                {
                    // If the course is not in the list, set CourseAssigned to false
                    mapping.CourseAssigned = false;
                    mapping.LastUpdated = DateTime.Now;
                }
                _dbContext.StudentCourses.Update(mapping);
            }

            // Add new mappings for remaining courses in the list
            foreach (var courseId in selectedCourses)
            {
                var newMapping = new StudentCourse
                {
                    StudentsSid = studentId,
                    CoursesCourseId = courseId,
                    CourseAssigned = true,
                    Created = DateTime.Now,
                    LastUpdated = DateTime.Now
                };
                _dbContext.StudentCourses.Add(newMapping);
            }

            // Save changes to the database
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

            List<CourseViewModel> cvmListSelected = SelectCourse(studentId, out List<StudentCourse> scList);
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
            // Step 1: Get the base list of students
            var query = from s in _dbContext.Students
                        select new
                        {
                            Student = s
                        };

            var result = query.ToList();

            // Step 2: Initialize the StudentViewModel list
            List<StudentViewModel> svmList = new List<StudentViewModel>();

            foreach (var item in result)
            {
                StudentViewModel svm = new StudentViewModel
                {
                    Sid = item.Student.Sid,
                    Name = item.Student.Name,
                    Email = item.Student.Email,
                    Phone = item.Student.Phone,
                    DateOfBirth = item.Student.DOB.ToString("dd MMM yyyy"),
                    Gender = item.Student.Gender == 1 ? "Male" : "Female"
                };
                svmList.Add(svm);
            }

            // Step 3: Get the list of students for the specified course and their student courses
            List<StudentViewModel> svmListSelected = SelectStudent(courseId, out List<StudentCourse> scList);

            // Step 4: Update the original list with course-related information
            foreach (var item1 in svmList)
            {
                foreach (var item2 in scList)
                {
                    if (item1.Sid == item2.StudentsSid && courseId == item2.CoursesCourseId && item2.CourseAssigned == true)
                    {
                        item1.isAssigned = true;
                    }
                }
            }

            return svmList;
        }

        public void MapStudentPost(int courseId, List<int> studentList)
        {
            // Fetch all existing mappings for the given courseId
            var existingMappings = _dbContext.StudentCourses.Where(sc => sc.CoursesCourseId == courseId).ToList();
            var courses = _dbContext.Courses.Where(c => c.CourseId == courseId).ToList();

            // Iterate over existing mappings
            foreach (var mapping in existingMappings)
            {
                if (studentList.Contains(mapping.StudentsSid))
                {
                    // If the student is in the list, ensure CourseAssigned is true
                    mapping.CourseAssigned = true;
                    mapping.LastUpdated = DateTime.Now;
                    studentList.Remove(mapping.StudentsSid); // Remove the student from the list as it's already mapped
                }
                else
                {
                    // If the student is not in the list, set CourseAssigned to false
                    mapping.CourseAssigned = false;
                    mapping.LastUpdated = DateTime.Now;
                }
                _dbContext.StudentCourses.Update(mapping);
            }

            // Add new mappings for remaining students in the list
            foreach (var student in studentList)
            {
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

            // Update isAssigned flag for courses
            _dbContext.Courses.UpdateRange(courses);

            // Save changes to the database
            _dbContext.SaveChanges();
        }


        public void UpdateSingleStudent(StudentCourseViewModel studentCourseViewModel)
        {
            var data =  _dbContext.StudentCourses
                .FirstOrDefault(x => x.StudentsSid == studentCourseViewModel.StudentsSid && x.CoursesCourseId == studentCourseViewModel.CoursesCourseId && x.CourseAssigned == studentCourseViewModel.CourseAssigned);

            Console.WriteLine("Data: "+data);
            if (data != null)
            {
                data.LastUpdated = DateTime.Now;
                data.CourseAssigned = !studentCourseViewModel.CourseAssigned;
                _dbContext.StudentCourses.Update(data);
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
        public void UpdateSingleCourse(StudentCourseViewModel studentCourseViewModel)
        {
            var existingMapping = _dbContext.StudentCourses.FirstOrDefault(x => x.StudentsSid == studentCourseViewModel.StudentsSid &&
                                                                                 x.CoursesCourseId == studentCourseViewModel.CoursesCourseId && x.CourseAssigned == studentCourseViewModel.CourseAssigned);
            if (existingMapping != null)
            {
                // If the mapping exists, update it
                existingMapping.LastUpdated = DateTime.Now;
                existingMapping.CourseAssigned = !studentCourseViewModel.CourseAssigned;
                _dbContext.StudentCourses.Update(existingMapping);
            }
            else
            {
                // If the mapping doesn't exist, create a new one
                StudentCourse newMapping = new StudentCourse
                {
                    StudentsSid = studentCourseViewModel.StudentsSid,
                    CoursesCourseId = studentCourseViewModel.CoursesCourseId,
                    Created = DateTime.Now,
                    CourseAssigned = studentCourseViewModel.CourseAssigned
                };

                _dbContext.StudentCourses.Add(newMapping);
            }

            _dbContext.SaveChanges();
        }


    }
}
    

