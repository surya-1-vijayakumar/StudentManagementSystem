using Microsoft.AspNetCore.Authorization;

namespace StudentManagementApi.Authorization
{
    public class AdminOnlyHandler
        : AuthorizationHandler<AdminOnlyRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AdminOnlyRequirement requirement)
        {
            if (context.User.Identity?.IsAuthenticated == true &&
                context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}