using System.Security.Claims;

namespace TaskManager.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/auth/me", (ClaimsPrincipal user) =>
            Results.Ok(new { username = user.FindFirstValue(ClaimTypes.Name) }))
            .RequireAuthorization()
            .WithTags("Auth");

        return app;
    }
}
