using Microsoft.EntityFrameworkCore;
using WebApplication_1.Data.DTOs;
using WebApplication_1.Models;

namespace WebApplication_1.Data.Repositories
{
    /// <summary>
    /// Repository for Student data access
    /// Handles all database operations for Student entity
    /// </summary>
    public interface IStudentRepository
    {
        Task<CreateStudentResultDto> CreateStudentAsync(CreateStudentDto createStudentDto);
        Task<Student?> GetStudentByIdAsync(int studentId);
        Task<List<Student>> GetAllStudentsAsync(int pageNumber = 1, int pageSize = 10);
        Task<bool> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int studentId);
        Task<bool> StudentExistsByEmailAsync(string email);
    }

    /// <summary>
    /// Repository implementation for Student data access using Stored Procedures
    /// </summary>
    public class StudentRepository : IStudentRepository
    {
        private readonly SchoolDbContext _context;
        private readonly ILogger<StudentRepository> _logger;

        public StudentRepository(SchoolDbContext context, ILogger<StudentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new student using sp_CreateStudent stored procedure
        /// Maps CreateStudentDto parameters to SQL SP parameters
        /// 
        /// SQL SP: sp_CreateStudent
        /// Parameters:
        ///   @StudentName VARCHAR(255) -> createStudentDto.StudentName
        ///   @Email VARCHAR(255) -> createStudentDto.Email
        ///   @ClassName VARCHAR(50) = NULL -> createStudentDto.ClassName
        ///   @Division VARCHAR(10) = NULL -> createStudentDto.Division
        ///   @PhoneNumber VARCHAR(20) = NULL -> createStudentDto.PhoneNumber
        /// 
        /// Return: StudentId (SCOPE_IDENTITY)
        /// </summary>
        public async Task<CreateStudentResultDto> CreateStudentAsync(CreateStudentDto createStudentDto)
        {
            try
            {
                _logger.LogInformation("Creating new student with email: {Email}", createStudentDto.Email);

                // Alternative 1: Using Raw SQL with Parameters (Recommended for SPs)
                var result = await _context.Database.SqlQueryRaw<CreateStudentResultDto>(
                    "EXEC dbo.sp_CreateStudent @StudentName = {0}, @Email = {1}, @ClassName = {2}, @Division = {3}, @PhoneNumber = {4}",
                    createStudentDto.StudentName,
                    createStudentDto.Email,
                    createStudentDto.ClassName ?? (object)DBNull.Value,
                    createStudentDto.Division ?? (object)DBNull.Value,
                    createStudentDto.PhoneNumber ?? (object)DBNull.Value
                ).ToListAsync();

                if (result.Count == 0)
                {
                    throw new InvalidOperationException("Failed to create student - stored procedure returned no result");
                }

                _logger.LogInformation("Student created successfully with ID: {StudentId}", result[0].StudentId);
                return result[0];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating student with email: {Email}", createStudentDto.Email);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a student by StudentId
        /// </summary>
        public async Task<Student?> GetStudentByIdAsync(int studentId)
        {
            try
            {
                _logger.LogInformation("Fetching student with ID: {StudentId}", studentId);
                return await _context.Students
                    .FirstOrDefaultAsync(s => s.StudentId == studentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching student with ID: {StudentId}", studentId);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all active students with pagination
        /// Uses LINQ for simplicity (can be replaced with sp_GetAllStudents SP later)
        /// </summary>
        public async Task<List<Student>> GetAllStudentsAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("Fetching all students - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

                return await _context.Students
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.StudentName)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching students");
                throw;
            }
        }

        /// <summary>
        /// Updates an existing student record
        /// </summary>
        public async Task<bool> UpdateStudentAsync(Student student)
        {
            try
            {
                _logger.LogInformation("Updating student with ID: {StudentId}", student.StudentId);

                var existingStudent = await _context.Students
                    .FirstOrDefaultAsync(s => s.StudentId == student.StudentId);

                if (existingStudent == null)
                {
                    _logger.LogWarning("Student not found with ID: {StudentId}", student.StudentId);
                    return false;
                }

                existingStudent.StudentName = student.StudentName;
                existingStudent.Email = student.Email;
                existingStudent.ClassName = student.ClassName;
                existingStudent.Division = student.Division;
                existingStudent.PhoneNumber = student.PhoneNumber;
                existingStudent.IsActive = student.IsActive;

                await _context.SaveChangesAsync();
                _logger.LogInformation("Student updated successfully with ID: {StudentId}", student.StudentId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student with ID: {StudentId}", student.StudentId);
                throw;
            }
        }

        /// <summary>
        /// Soft delete - marks student as inactive
        /// </summary>
        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            try
            {
                _logger.LogInformation("Deleting student with ID: {StudentId}", studentId);

                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.StudentId == studentId);

                if (student == null)
                {
                    _logger.LogWarning("Student not found with ID: {StudentId}", studentId);
                    return false;
                }

                student.IsActive = false;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Student deleted successfully with ID: {StudentId}", studentId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student with ID: {StudentId}", studentId);
                throw;
            }
        }

        /// <summary>
        /// Checks if a student exists by email
        /// </summary>
        public async Task<bool> StudentExistsByEmailAsync(string email)
        {
            try
            {
                return await _context.Students
                    .AnyAsync(s => s.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking student existence by email: {Email}", email);
                throw;
            }
        }
    }
}
