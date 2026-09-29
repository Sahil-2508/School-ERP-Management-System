using System.ComponentModel.DataAnnotations;

namespace WebApplication_1.Models
{
    public class ExamMaster
    {
        public int ExaminationID { get; set; }

        [Required]
        [Display(Name = "Exam Code")]
        public string ExamCode { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Exam Name")]
        public string ExamName { get; set; } = string.Empty;

        [Display(Name = "Exam Type")]
        public string? ExamType { get; set; }

        [Display(Name = "Semester")]
        public string? Semester { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Sequence")]
        public int SequenceNo { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Weightage")]
        public decimal Weightage { get; set; }

        [Display(Name = "Published")]
        public bool IsPublished { get; set; }

        [Display(Name = "Result Published")]
        public bool IsResultPublished { get; set; }

        [Display(Name = "Locked")]
        public bool IsLocked { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }
}
