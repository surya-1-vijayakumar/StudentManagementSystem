
using StudentManagementApi.Models;

namespace StudentManagementApi.Repositories.Interfaces;

public interface IStudentRepository
{
    Task<List<Student>> GetAllAsync(string? search);

    Task<Student?> GetByIdAsync(int id);

    Task<bool> EmailExistsAsync(string email, int? excludeId = null);

    Task AddAsync(Student student);

    Task UpdateAsync(Student student);

    Task DeleteAsync(Student student);
}
