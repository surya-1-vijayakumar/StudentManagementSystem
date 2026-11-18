
using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Services;

namespace StudentManagementApi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DuplicateEmailException ex)
        {
            await WriteError(context, 409, ex.Message);
        }
        catch (DbUpdateException)
        {
            await WriteError(context, 409, "Database conflict occurred.");
        }
        catch (Exception)
        {
            await WriteError(context, 500, "Internal server error.");
        }
    }

    private static async Task WriteError(
        HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(
            new { message }
        );
    }
}
