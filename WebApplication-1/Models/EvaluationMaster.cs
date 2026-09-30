using System.ComponentModel.DataAnnotations;

namespace WebApplication_1.Models
{
    public class EvaluationMaster
    {
        public int EvaluationID { get; set; }

        [Required]
        [Display(Name = "Evaluation Code")]
        public string EvaluationCode { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Evaluation Name")]
        public string EvaluationName { get; set; } = string.Empty;

        [Display(Name = "Evaluation Type")]
        public string? EvaluationType { get; set; }

        [Display(Name = "Maximum Marks")]
        [Range(0, 1000)]
        public decimal MaximumMarks { get; set; }

        [Display(Name = "Pass Marks")]
        [Range(0, 1000)]
        public decimal PassMarks { get; set; }

        [Display(Name = "Weightage %")]
        [Range(0, 100)]
        public decimal Weightage { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Sequence")]
        public int SequenceNo { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }
    }
}
