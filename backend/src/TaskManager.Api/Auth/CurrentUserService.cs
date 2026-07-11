using System.Security.Claims;
using TaskManager.Application.Interfaces;

namespace TaskManager.Api.Auth;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _accessor;

    public CurrentUserService(IHttpContextAccessor accessor) => _accessor = accessor;

    public Guid UserId
    {
        get
        {
            var value = _accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(value, out var id)) return id;
            throw new UnauthorizedAccessException("Authenticated user context is missing.");
        }
    }
}
