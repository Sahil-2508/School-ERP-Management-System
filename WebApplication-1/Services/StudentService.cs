using WebApplication_1.Data.DTOs;
using WebApplication_1.Data.Repositories;
using WebApplication_1.Models;

namespace WebApplication_1.Services
{
    /// <summary>
    /// Service layer for Student business logic
    /// Handles validation and coordination with repository
    /// </summary>
    public interface IStudentService
    {
        Task<(bool Success, int StudentId, string Message)> CreateStudentAsync(CreateStudentDto createStudentDto);
        Task<Student?> GetStudentByIdAsync(int studentId);
        Task<List<Student>> GetAllStudentsAsync(int pageNumber = 1, int pageSize = 10);
        Task<(bool Success, string Message)> UpdateStudentAsync(Student student);
        Task<(bool Success, string Message)> DeleteStudentAsync(int studentId);
    }


    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IStudentRepository studentRepository, ILogger<StudentService> logger)
        {
            _studentRepository = studentRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new student with validation
        /// 
        /// Validation Flow:
        /// 1. Validate StudentName is not empty
        /// 2. Validate Email format and uniqueness
        /// 3. Call repository to execute sp_CreateStudent
        /// 4. Return result with StudentId or error message
        /// 
        /// Maps to: sp_CreateStudent stored procedure
        /// </summary>
        public async Task<(bool Success, int StudentId, string Message)> CreateStudentAsync(CreateStudentDto createStudentDto)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(createStudentDto.StudentName))
                {
                    return (false, 0, "Student name is required.");
                }

                if (string.IsNullOrWhiteSpace(createStudentDto.Email))
                {
                    return (false, 0, "Email is required.");
                }

                // Validate email format
                if (!IsValidEmail(createStudentDto.Email))
                {
                    return (false, 0, "Email format is invalid.");
                }

                // Check if email already exists
                if (await _studentRepository.StudentExistsByEmailAsync(createStudentDto.Email))
                {
                    return (false, 0, $"A student with email '{createStudentDto.Email}' already exists.");
                }

                // Call repository to execute stored procedure
                var result = await _studentRepository.CreateStudentAsync(createStudentDto);

                _logger.LogInformation("Student created successfully. StudentId: {StudentId}, Email: {Email}",
                    result.StudentId, createStudentDto.Email);

                return (true, result.StudentId, "Student created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating student with email: {Email}", createStudentDto.Email);
                return (false, 0, $"An error occurred while creating the student: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a student by ID
        /// </summary>
        public async Task<Student?> GetStudentByIdAsync(int studentId)
        {
            try
            {
                return await _studentRepository.GetStudentByIdAsync(studentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving student with ID: {StudentId}", studentId);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all students with pagination
        /// </summary>
        public async Task<List<Student>> GetAllStudentsAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 100) pageSize = 100; // Max page size

                return await _studentRepository.GetAllStudentsAsync(pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving students");
                throw;
            }
        }

        /// <summary>
        /// Updates a student record with validation
        /// </summary>
        public async Task<(bool Success, string Message)> UpdateStudentAsync(Student student)
        {
            try
            {
                // Validation
                if (student.StudentId <= 0)
                {
                    return (false, "Invalid student ID.");
                }

                if (string.IsNullOrWhiteSpace(student.StudentName))
                {
                    return (false, "Student name is required.");
                }

                if (string.IsNullOrWhiteSpace(student.Email))
                {
                    return (false, "Email is required.");
                }

                if (!IsValidEmail(student.Email))
                {
                    return (false, "Email format is invalid.");
                }

                var result = await _studentRepository.UpdateStudentAsync(student);

                if (!result)
                {
                    return (false, "Student not found.");
                }

                _logger.LogInformation("Student updated successfully. StudentId: {StudentId}", student.StudentId);
                return (true, "Student updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student with ID: {StudentId}", student.StudentId);
                return (false, $"An error occurred while updating the student: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a student (soft delete - marks as inactive)
        /// </summary>
        public async Task<(bool Success, string Message)> DeleteStudentAsync(int studentId)
        {
            try
            {
                if (studentId <= 0)
                {
                    return (false, "Invalid student ID.");
                }

                var result = await _studentRepository.DeleteStudentAsync(studentId);

                if (!result)
                {
                    return (false, "Student not found.");
                }

                _logger.LogInformation("Student deleted successfully. StudentId: {StudentId}", studentId);
                return (true, "Student deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student with ID: {StudentId}", studentId);
                return (false, $"An error occurred while deleting the student: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates email format
        /// </summary>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
