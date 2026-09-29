using Microsoft.AspNetCore.Mvc;
using WebApplication_1.Models;

namespace WebApplication_1.Controllers
{
    public class ExaminationController : Controller
    {
        public IActionResult Index()
        {
            var exams = new List<ExamMaster>
            {
                new ExamMaster
                {
                    ExaminationID = 1,
                    ExamCode = "MID-2026",
                    ExamName = "Mid Term Examination",
                    ExamType = "Theory",
                    Semester = "Semester 1",
                    StartDate = new DateTime(2026, 10, 1),
                    EndDate = new DateTime(2026, 10, 15),
                    SequenceNo = 1,
                    Weightage = 30,
                    IsPublished = false,
                    IsResultPublished = false,
                    IsLocked = false,
                    IsActive = true
                },

                new ExamMaster
                {
                    ExaminationID = 2,
                    ExamCode = "FINAL-2026",
                    ExamName = "Final Examination",
                    ExamType = "Theory",
                    Semester = "Semester 2",
                    StartDate = new DateTime(2027, 2, 1),
                    EndDate = new DateTime(2027, 2, 15),
                    SequenceNo = 2,
                    Weightage = 70,
                    IsPublished = false,
                    IsResultPublished = false,
                    IsLocked = false,
                    IsActive = true
                }
            };

            return View(exams);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ExamMaster model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData["SuccessMessage"] =
                "Examination created successfully.";

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            return View();
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            TempData["SuccessMessage"] =
                "Examination deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}

