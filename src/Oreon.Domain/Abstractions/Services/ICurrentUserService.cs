namespace Oreon.Domain.Abstractions.Services;

/// <summary>
/// Abstraction for accessing current authenticated user information.
/// Domain layer uses this interface without knowing about ASP.NET Core Identity implementation.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the unique identifier of the current authenticated user.
    /// </summary>
    /// <returns>User ID as Guid, or Guid.Empty if not authenticated.</returns>
    Guid GetUserId();

    /// <summary>
    /// Gets the username of the current authenticated user.
    /// </summary>
    /// <returns>Username, or null if not authenticated.</returns>
    string? GetUsername();

    /// <summary>
    /// Gets the email of the current authenticated user.
    /// </summary>
    /// <returns>Email, or null if not authenticated.</returns>
    string? GetEmail();

    /// <summary>
    /// Checks if the current user is authenticated.
    /// </summary>
    /// <returns>True if user is authenticated, otherwise false.</returns>
    bool IsAuthenticated();

    /// <summary>
    /// Checks if the current user has a specific role.
    /// </summary>
    /// <param name="role">Role name to check.</param>
    /// <returns>True if user has the role, otherwise false.</returns>
    bool IsInRole(string role);

    /// <summary>
    /// Gets all roles of the current authenticated user.
    /// </summary>
    /// <returns>Collection of role names.</returns>
    IReadOnlyCollection<string> GetRoles();
}
