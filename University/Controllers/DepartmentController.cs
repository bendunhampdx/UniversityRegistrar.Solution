using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using University.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace University.Controllers
{
  public class DepartmentsController : Controller
  {
    private readonly UniversityContext _db;
    public DepartmentsController(UniversityContext db)
    {
      _db = db;
    }
    public ActionResult Index()
    {
      List<Department> model = _db.Departments.ToList();
      return View(model);
    }
    public ActionResult Create()
    {
      return View();
    }
    [HttpPost]
    public ActionResult Create(Department department)
    {
      _db.Departments.Add(department);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Details(int id)
    {
      ViewBag.StudentId = new SelectList(_db.Students, "StudentId", "Name");
      ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name");
      ViewBag.NoCourses = _db.Courses.ToList().Count == 0;
      ViewBag.NoStudents = _db.Students.ToList().Count == 0;
      var thisDepartment = _db.Departments 
      .Include(department => department.CourseJoinEntities)
      .ThenInclude(join => join.Course)
      .Include(department => department.StudentJoinEntities)
      .ThenInclude(join => join.Student)
      .FirstOrDefault(department => department.DepartmentId == id);
      return View(thisDepartment);
    }
        public ActionResult Edit(int id)
    {
      return View(_db.Departments.FirstOrDefault(department => department.DepartmentId == id));
    }

    [HttpPost]
    public ActionResult Edit(Department department)
    {
      _db.Entry(department).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      return View(_db.Departments.FirstOrDefault(department => department.DepartmentId == id));
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisDepartment = _db.Departments.FirstOrDefault(department => department.DepartmentId == id);
      _db.Departments.Remove(thisDepartment);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult AddStudent(Department Department, int StudentId)
    {
      if (StudentId != 0)
      {
        _db.DepartmentStudent.Add(new DepartmentStudent() { StudentId = StudentId, DepartmentId = Department.DepartmentId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteStudent(int joinId)
    {
      var joinEntry = _db.DepartmentStudent.FirstOrDefault(entry => entry.DepartmentStudentId == joinId);
      _db.DepartmentStudent.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult AddCourse(Department Department, int CourseId)
    {
      if (CourseId != 0)
      {
        _db.CourseDepartment.Add(new CourseDepartment() { CourseId = CourseId, DepartmentId = Department.DepartmentId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteCourse(int joinId)
    {
      var joinEntry = _db.CourseDepartment.FirstOrDefault(entry => entry.CourseDepartmentId == joinId);
      _db.CourseDepartment.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}