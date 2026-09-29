using System.ComponentModel.DataAnnotations;

namespace WebApplication_1.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required]
        [Display(Name = "Student Name")]
        public string StudentName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Class")]
        public string ClassName { get; set; } = string.Empty;

        public string Division { get; set; } = string.Empty;

        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
