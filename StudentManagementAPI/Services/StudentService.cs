
using StudentManagementApi.DTOs;
using StudentManagementApi.Models;
using StudentManagementApi.Repositories.Interfaces;
using StudentManagementApi.Services.Interfaces;

namespace StudentManagementApi.Services;

public class DuplicateEmailException : Exception
{
    public DuplicateEmailException()
        : base("Email already exists.")
    {
    }
}

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;

    public StudentService(IStudentRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Student>> GetAllAsync(string? search)
    {
        return _repository.GetAllAsync(search);
    }

    public Task<Student?> GetByIdAsync(int id)
    {
        return _repository.GetByIdAsync(id);
    }

    public async Task<Student> CreateAsync(CreateStudentDto dto)
    {
        if (await _repository.EmailExistsAsync(dto.Email))
            throw new DuplicateEmailException();

        var student = new Student
        {
            Name = dto.Name,
            Email = dto.Email,
            Department = dto.Department,
            Age = dto.Age
        };

        await _repository.AddAsync(student);

        return student;
    }

    public async Task<Student?> UpdateAsync(
        int id, UpdateStudentDto dto)
    {
        var student = await _repository.GetByIdAsync(id);

        if (student == null)
            return null;

        if (await _repository.EmailExistsAsync(dto.Email, id))
            throw new DuplicateEmailException();

        student.Name = dto.Name;
        student.Email = dto.Email;
        student.Department = dto.Department;
        student.Age = dto.Age;

        await _repository.UpdateAsync(student);

        return student;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var student = await _repository.GetByIdAsync(id);

        if (student == null)
            return false;

        await _repository.DeleteAsync(student);

        return true;
    }
}
