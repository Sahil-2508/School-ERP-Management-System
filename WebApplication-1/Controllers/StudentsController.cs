using Microsoft.AspNetCore.Mvc;
using WebApplication_1.Data.DTOs;
using WebApplication_1.Models;
using WebApplication_1.Services;

namespace WebApplication_1.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        // GET: /Students
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            try
            {
                ViewBag.PageTitle = "Student Management";
                var students = await _studentService.GetAllStudentsAsync(pageNumber, 10);
                return View(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading students");
                TempData["ErrorMessage"] = "An error occurred while loading students.";
                return View(new List<Student>());
            }
        }

        // GET: /Students/Details/1
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);

                if (student == null)
                {
                    return NotFound();
                }

                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading student details");
                TempData["ErrorMessage"] = "An error occurred while loading student details.";
                return RedirectToAction(nameof(Index));
            }
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
        public async Task<IActionResult> Create(CreateStudentDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var (success, studentId, message) = await _studentService.CreateStudentAsync(model);

                if (!success)
                {
                    ModelState.AddModelError("", message);
                    return View(model);
                }

                TempData["SuccessMessage"] = $"Student created successfully (ID: {studentId}).";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating student");
                ModelState.AddModelError("", "An error occurred while creating the student.");
                return View(model);
            }
        }

        // GET: /Students/Edit/1
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);

                if (student == null)
                {
                    return NotFound();
                }

                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading student for edit");
                TempData["ErrorMessage"] = "An error occurred while loading the student.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Students/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Student student)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(student);
                }

                var (success, message) = await _studentService.UpdateStudentAsync(student);

                if (!success)
                {
                    ModelState.AddModelError("", message);
                    return View(student);
                }

                TempData["SuccessMessage"] = "Student updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student");
                ModelState.AddModelError("", "An error occurred while updating the student.");
                return View(student);
            }
        }

        // POST: /Students/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var (success, message) = await _studentService.DeleteStudentAsync(id);

                if (!success)
                {
                    TempData["ErrorMessage"] = message;
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Student deleted successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student");
                TempData["ErrorMessage"] = "An error occurred while deleting the student.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
