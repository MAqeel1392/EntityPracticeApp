using EntityPracticeApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using EntityPracticeApp.Models;
using EntityPracticeApp.Service;
using EntityPracticeApp.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace EntityPracticeApp.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IStudentCourseService _studentCourseService;
        public StudentsController(IStudentService studentService, IStudentCourseService studentCourseService)
        {
            _studentService = studentService;
            _studentCourseService = studentCourseService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<StudentViewModel> students = await _studentService.GetAllStudent();
            return View(students);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(StudentViewModel student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            try
            {
                _studentService.AddStudent(student);
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("Email already exists"))
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(student);
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpGet]
        public  IActionResult Edit(int id)
        {
            StudentViewModel svm = new StudentViewModel();
            svm.Sid = id;
            StudentViewModel student = _studentService.GetStudent(svm);
            if (student != null)
            {
                return View(student);
            }
            else
            {
                // Handle case where student is not found
                // For example, return a 404 Not Found response
                return NotFound();
            }
        }
        [HttpPost]
        public IActionResult EditPost(StudentViewModel student)
        {
            //var student1 = await _dbContext.Students.Where(x => x.Sid == id).FirstOrDefaultAsync();
            //student.Sid = student1.Sid;
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            _studentService.UpdateStudent(student);
            return RedirectToAction("Index");

        }
        public IActionResult Delete(int id)
        {
            _studentService.DeleteStudent(id);

            return RedirectToAction("Index"); // Assuming "Details" is the action name
        }
        public IActionResult ShowCourses(int id)
        {
            StudentViewModel svm = new StudentViewModel();
            svm.Sid = id;
            var student = _studentService.GetStudent(svm);
            ViewBag.StudentName =  student.Name;
            List<CourseViewModel> courses = _studentCourseService.SelectCourse(id);
            return View(courses);
        }
        public IActionResult MapCourses(int id)
        {
            TempData["studentId"] = id;
            StudentViewModel svm = new StudentViewModel();
            svm.Sid = id;
            var student = _studentService.GetStudent(svm);
            ViewBag.StudentName = student.Name;

            List<CourseViewModel> courses = _studentCourseService.MapCourse(id);
            return View(courses);
        }
        [HttpPost]
        public IActionResult MapCourses(int studentId, List<int> selectedCourses)
        {
            _studentCourseService.PostMapCourses(studentId, selectedCourses);
            return RedirectToAction("Index");
        }


    }
}
