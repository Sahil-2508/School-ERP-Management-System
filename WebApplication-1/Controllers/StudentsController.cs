using Microsoft.AspNetCore.Mvc;
using WebApplication_1.Models;

namespace WebApplication_1.Controllers
{
    public class StudentsController : Controller
    {
        private static List<Student> students = new()
        {
            new Student
            {
                StudentId = 1,
                StudentName = "Rahul Patil",
                Email = "rahul@gmail.com",
                ClassName = "10th",
                Division = "A",
                PhoneNumber = "9876543210",
                IsActive = true
            },

            new Student
            {
                StudentId = 2,
                StudentName = "Priya Sharma",
                Email = "priya@gmail.com",
                ClassName = "9th",
                Division = "B",
                PhoneNumber = "9876543211",
                IsActive = true
            },

            new Student
            {
                StudentId = 3,
                StudentName = "Amit Joshi",
                Email = "amit@gmail.com",
                ClassName = "8th",
                Division = "A",
                PhoneNumber = "9876543212",
                IsActive = false
            }
        };

        // GET: /Students
        public IActionResult Index()
        {
            ViewBag.PageTitle = "Student Management";

            return View(students);
        }

        // GET: /Students/Details/1
        public IActionResult Details(int id)
        {
            var student = students.FirstOrDefault(x => x.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: /Students/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            student.StudentId = students.Count + 1;

            students.Add(student);

            TempData["SuccessMessage"] =
                "Student created successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: /Students/Edit/1
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student = students.FirstOrDefault(x => x.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: /Students/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            var existingStudent =
                students.FirstOrDefault(x =>
                    x.StudentId == student.StudentId);

            if (existingStudent == null)
            {
                return NotFound();
            }

            existingStudent.StudentName = student.StudentName;
            existingStudent.Email = student.Email;
            existingStudent.ClassName = student.ClassName;
            existingStudent.Division = student.Division;
            existingStudent.PhoneNumber = student.PhoneNumber;
            existingStudent.IsActive = student.IsActive;

            TempData["SuccessMessage"] =
                "Student updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // POST: /Students/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var student =
                students.FirstOrDefault(x => x.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            students.Remove(student);

            TempData["SuccessMessage"] =
                "Student deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
