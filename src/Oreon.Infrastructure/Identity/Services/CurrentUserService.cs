using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Oreon.Domain.Abstractions.Services;

namespace Oreon.Infrastructure.Identity.Services;

/// <summary>
/// Implementation of ICurrentUserService using ASP.NET Core HttpContext.
/// This service extracts user information from JWT claims or authentication cookies.
/// </summary>
public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public Guid GetUserId()
    {
        var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim))
            return Guid.Empty;

        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }

    public string? GetUsername()
    {
        // ClaimTypes.Name typically contains the username in JWT
        return User?.FindFirst(ClaimTypes.Name)?.Value;
    }

    public string? GetEmail()
    {
        return User?.FindFirst(ClaimTypes.Email)?.Value;
    }

    public bool IsAuthenticated()
    {
        return User?.Identity?.IsAuthenticated ?? false;
    }

    public bool IsInRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            return false;

        return User?.IsInRole(role) ?? false;
    }

    public IReadOnlyCollection<string> GetRoles()
    {
        if (User is null)
            return Array.Empty<string>();

        var roles = User.FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        return roles.AsReadOnly();
    }
}
