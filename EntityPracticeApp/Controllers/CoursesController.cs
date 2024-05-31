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
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IStudentCourseService _studentCourseService;

        public CoursesController(ICourseService courseService, IStudentCourseService studentCourseService)
        {
            _courseService = courseService;
            _studentCourseService = studentCourseService;
        }

        public async Task<IActionResult> Index()
        {

            List<CourseViewModel> courses =  await _courseService.GetAllCourses();
            return View(courses);
        }
        public IActionResult Add() {
            return View();
        }
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
         
        public  IActionResult Edit(int id)
        {

            CourseViewModel cvm = new CourseViewModel();
            cvm.CourseId = id;
            cvm = _courseService.GetCourse(cvm);
            return View(cvm);
        }
        [HttpPost]
        public IActionResult EditPost(CourseViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }
            _courseService.UpdateCourse(cvm);
            return RedirectToAction("Index");

           
        }
        public IActionResult Delete (int id)
        {
            _courseService.RemoveCourse(id);
            return RedirectToAction("Index");
        }
        public IActionResult ShowStudent(int id)
        { 
            CourseViewModel cvm = new CourseViewModel();
            cvm.CourseId=id;
            var course = _courseService.GetCourse(cvm);
            ViewBag.CourseName = course.Title;
            List<StudentViewModel> students = _studentCourseService.SelectStudent(id, out List<StudentCourse> scList);
            return View(students);
        }
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

        [HttpPost]
        public IActionResult MapStudentsPost(int courseId, List<int>selectedStudents)
        {
                TempData["courseId"] = courseId;
            _studentCourseService.MapStudentPost(courseId, selectedStudents);

            return RedirectToAction("Index");
        }
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
