
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StudentManagementApi.DTOs;
using StudentManagementApi.Services.Interfaces;

namespace StudentManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "AdminOnly")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _service;

    public StudentsController(IStudentService service)
    {
        _service = service;
    }

    // GET: api/students?search=computer
    [HttpGet]
    public async Task<IActionResult> GetAll(string? search)
    {
        var students = await _service.GetAllAsync(search);
        return Ok(students);
    }

    // GET: api/students/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var student = await _service.GetByIdAsync(id);

        if (student == null)
            return NotFound(new { message = "Student not found." });

        return Ok(student);
    }

    // POST: api/students
    [HttpPost]
    public async Task<IActionResult> Create(CreateStudentDto dto)
    {
        var student = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = student.Id },
            student
        );
    }

    // PUT: api/students/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id, UpdateStudentDto dto)
    {
        var student = await _service.UpdateAsync(id, dto);

        if (student == null)
            return NotFound(new { message = "Student not found." });

        return Ok(student);
    }

    // DELETE: api/students/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = "Student not found." });

        return NoContent();
    }
}
