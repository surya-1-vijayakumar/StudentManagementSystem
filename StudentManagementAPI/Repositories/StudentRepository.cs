
using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Data;
using StudentManagementApi.Models;
using StudentManagementApi.Repositories.Interfaces;

namespace StudentManagementApi.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _context;

    public StudentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Student>> GetAllAsync(string? search)
    {
        var query = _context.Students.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(s =>
                s.Name.Contains(search) ||
                s.Department.Contains(search));
        }

        return await query.ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        return await _context.Students.FindAsync(id);
    }

    public async Task<bool> EmailExistsAsync(
        string email, int? excludeId = null)
    {
        return await _context.Students.AnyAsync(s =>
            s.Email == email &&
            (!excludeId.HasValue || s.Id != excludeId.Value));
    }

    public async Task AddAsync(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Student student)
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Student student)
    {
        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
    }
}
