using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Data;
using StudentManagementApi.DTOs;
using StudentManagementApi.Models;
using StudentManagementApi.Services;

namespace StudentManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthController(
            AppDbContext context,
            TokenService tokenService,
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == dto.Email);

            if (emailExists)
            {
                return Conflict(new
                {
                    message = "Email already registered."
                });
            }

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Role = "Student"
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user, dto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return StatusCode(201, new
            {
                message = "Registration successful."
            });
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            var token = _tokenService.GenerateToken(user);

            return Ok(new AuthResponseDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Token = token
            });
        }
    }
}