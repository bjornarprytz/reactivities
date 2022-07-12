using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security;

public class IsHostRequirement : IAuthorizationRequirement
{
}

public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
{
    private readonly DataContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public IsHostRequirementHandler(
        DataContext dbContext,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null
            ||
            _httpContextAccessor.HttpContext?.Request.RouteValues.SingleOrDefault(x => x.Key == "id").Value?.ToString() is not { } guid
        ) return;

        var activityId = Guid.Parse(guid);

        if (await _dbContext.ActivityAttendees.AsNoTracking().SingleOrDefaultAsync(x => x.AppUserId == userId && x.ActivityId == activityId)
            is { IsHost: true })
        {
            context.Succeed(requirement);
        }

        return;
    }
}