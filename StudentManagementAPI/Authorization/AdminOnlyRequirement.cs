using Microsoft.AspNetCore.Authorization;

namespace StudentManagementApi.Authorization
{
    public class AdminOnlyRequirement : IAuthorizationRequirement
    {
    }
}