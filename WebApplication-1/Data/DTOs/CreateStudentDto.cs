namespace WebApplication_1.Data.DTOs
{   
    public class CreateStudentDto
    {
        public string StudentName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
               
        public string? ClassName { get; set; }
                
        public string? Division { get; set; }
               
        public string? PhoneNumber { get; set; }
    }

    
    public class CreateStudentResultDto
    {
        public int StudentId { get; set; }
    }
}
