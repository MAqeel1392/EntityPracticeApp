﻿using EntityPracticeApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using EntityPracticeApp.Models;
using EntityPracticeApp.Service;
using EntityPracticeApp.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using EntityPracticeApp.Migrations;

namespace EntityPracticeApp.Controllers
{
    //[Authorize]
    [Authorize(Roles = "Admin")]
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
        //[Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        //[Authorize(Roles = "admin")]
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

        //[Authorize(Roles = "admin")]
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
        //[Authorize(Roles = "admin")]
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
        //[Authorize(Roles = "admin")]
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
            List<CourseViewModel> courses = _studentCourseService.SelectCourse(id, out List<StudentCourse> scList);
            return View(courses);
        }
        //[Authorize(Roles = "admin")]
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
        //[Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult MapCoursesPost(int studentId, List<int> selectedCourses)
        {
            TempData["studentId"] = studentId;
            _studentCourseService.PostMapCourses(studentId, selectedCourses);
            return RedirectToAction("Index");
        }
        //[Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult UpdateMapping(int studentId, int courseId, bool isSelected)
        {
            var studentCourse = new StudentCourseViewModel
            {
                StudentsSid = studentId,
                CoursesCourseId = courseId,
                CourseAssigned = isSelected
            };
            _studentCourseService.UpdateSingleCourse(studentCourse);
            TempData["studentId"] = studentId;
            TempData.Keep();

            return View(MapCoursesResult(studentId));
        }
        //[Authorize(Roles = "admin")]
        public List<CourseViewModel> MapCoursesResult(int id)
        {
            List<CourseViewModel> courses = _studentCourseService.MapCourse(id);
            StudentViewModel svm = new StudentViewModel();
            svm.Sid = id;
            var student = _studentService.GetStudent(svm);
            ViewBag.StudentName = student.Name;
            TempData["StudentId"] = id;
            return courses;
        }

    }
}
