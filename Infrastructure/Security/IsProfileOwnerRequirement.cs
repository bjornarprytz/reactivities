using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security;

public class IsProfileOwnerRequirement : IAuthorizationRequirement { }

public class IsProfileOwnerRequirementHandler : AuthorizationHandler<IsProfileOwnerRequirement>
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IsProfileOwnerRequirementHandler(
        DataContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsProfileOwnerRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null
            ||
            _httpContextAccessor.HttpContext?.Request.RouteValues
                .SingleOrDefault(x => x.Key == "username").Value?.ToString() 
                is not { } username)
        {
            return;
        }
        
        if (await _context.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == userId)
            is not { } user
            ||
            !user.UserName.Equals(username, StringComparison.InvariantCulture))
        {
            return;
        }
        
        context.Succeed(requirement);
    }
}