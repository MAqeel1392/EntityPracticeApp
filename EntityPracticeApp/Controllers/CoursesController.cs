using EntityPracticeApp.Migrations;
using EntityPracticeApp.Models;
using EntityPracticeApp.Service;
using EntityPracticeApp.Service.Interface;
using EntityPracticeApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EntityPracticeApp.Controllers
{
    //[Authorize(Roles =SD.Role_Customer)]
    //[Authorize]
    //[Authorize(Roles = "admin")]
    //[Authorize(Roles = "client")]
    [Authorize(Roles = "Admin")]
    public class CoursesController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IStudentCourseService _studentCourseService;

        public CoursesController(ICourseService courseService, IStudentCourseService studentCourseService)
        {
            _courseService = courseService;
            _studentCourseService = studentCourseService;
        }

        //[Authorize()]
        //[Authorize(Roles = "client")]
        public async Task<IActionResult> Index()
        {

            List<CourseViewModel> courses =  await _courseService.GetAllCourses();
            return View(courses);
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult Add() {
            return View();
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add(CourseViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }

            try
            {
                _courseService.AddCourse(cvm);
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("Course Title already exists"))
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(cvm);
                }
                else
                {
                    throw;
                }
            }

        }
        //[Authorize(Roles = "Admin")]
        public  IActionResult Edit(int id)
        {

            CourseViewModel cvm = new CourseViewModel();
            cvm.CourseId = id;
            cvm = _courseService.GetCourse(cvm);
            return View(cvm);
        }
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditPost(CourseViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }

            try
            {
                _courseService.UpdateCourse(cvm);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception (assumes you have a logging mechanism in place)
                // _logger.LogError(ex, "Error updating course with ID {CourseId}", cvm.CourseId);

                // Optionally, add a model error to notify the user
                ModelState.AddModelError("", "An error occurred while updating the course. Please try again.");

                return View(cvm);
            }
        }
        //[Authorize(Roles = "Admin")]
        public IActionResult Delete (int id)
        {
            _courseService.RemoveCourse(id);
            return RedirectToAction("Index");
        }
        //[Authorize(Roles = "client")]
        public IActionResult ShowStudent(int id)
        { 
            CourseViewModel cvm = new CourseViewModel();
            cvm.CourseId=id;
            var course = _courseService.GetCourse(cvm);
            ViewBag.CourseName = course.Title;
            List<StudentViewModel> students = _studentCourseService.SelectStudent(id, out List<StudentCourse> scList);
            return View(students);
        }
        //[Authorize(Roles = "Admin")]
        public IActionResult MapStudents(int id)
        {


            List<StudentViewModel> students = _studentCourseService.MapStudent(id);
            CourseViewModel cvm = new CourseViewModel();
            cvm.CourseId = id;
            var course = _courseService.GetCourse(cvm);
            ViewBag.CourseName = course.Title;
            TempData["courseId"] = id;
            return View(students);

            //List<StudentViewModel> students = _studentCourseService.MapStudent(id);
            //CourseViewModel cvm = _courseService.GetCourse(new CourseViewModel { CourseId = id });
            //ViewBag.CourseName = cvm.Title;
            //TempData["courseId"] = id;
            //return View(students);
        }
        //[Authorize(Roles = "Admin")]
        public List<StudentViewModel> MapStudentsResult(int id)
        {


            List<StudentViewModel> students = _studentCourseService.MapStudent(id);
            CourseViewModel cvm = new CourseViewModel();
            cvm.CourseId = id;
            var course = _courseService.GetCourse(cvm);
            ViewBag.CourseName = course.Title;
            TempData["courseId"] = id;
            return students;

            //List<StudentViewModel> students = _studentCourseService.MapStudent(id);
            //CourseViewModel cvm = _courseService.GetCourse(new CourseViewModel { CourseId = id });
            //ViewBag.CourseName = cvm.Title;
            //TempData["courseId"] = id;
            //return View(students);
        }
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult MapStudentsPost(int courseId, List<int>selectedStudents)
        {
                TempData["courseId"] = courseId;
            _studentCourseService.MapStudentPost(courseId, selectedStudents);

            return RedirectToAction("Index");
        }
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public  IActionResult UpdateMapping(int sid, int courseId, bool isSelected)
        {
            var studentCourse = new StudentCourseViewModel
            {
                StudentsSid = sid,
                CoursesCourseId = courseId,
                CourseAssigned = isSelected
            };
            _studentCourseService.UpdateSingleStudent(studentCourse);
            TempData["courseId"] = courseId;
            TempData.Keep();

            return View(MapStudentsResult(courseId));
            //return RedirectToAction("MapStudents", courseId );
        }
        //[HttpPost]
        //public IActionResult MapStudent(int sid, int cid)
        //{
        //    _studentCourseService.MapSingleStudent(sid, cid);
        //    return RedirectToAction("MapStudents");
        //}

    }
}
