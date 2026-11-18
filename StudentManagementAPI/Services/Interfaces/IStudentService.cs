
using StudentManagementApi.DTOs;
using StudentManagementApi.Models;

namespace StudentManagementApi.Services.Interfaces;

public interface IStudentService
{
    Task<List<Student>> GetAllAsync(string? search);

    Task<Student?> GetByIdAsync(int id);

    Task<Student> CreateAsync(CreateStudentDto dto);

    Task<Student?> UpdateAsync(int id, UpdateStudentDto dto);

    Task<bool> DeleteAsync(int id);
}
