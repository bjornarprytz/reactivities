using Application.Core;
using Application.Interfaces;
using Domain;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Unit = MediatR.Unit;

namespace Application.Followers;

public class FollowToggle
{
    public record Command(string TargetUsername) : IRequestWrapper;
    
    public class Handler : IRequestHandlerWrapper<Command>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }
        
        public async Task<Either<Error, Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            
            var username = _userAccessor.GetUsername();

            if (request.TargetUsername == username)
            {
                return new Error("Self-following is not allowed");
            }

            if (await _context.Users.FirstOrDefaultAsync(x => x.UserName == username, cancellationToken)
                is not { } observer
                ||
                await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.TargetUsername, cancellationToken)
                is not { } target
                )
            {
                return default;
            }
            
            if (await _context.UserFollowings.FindAsync(new object?[] { observer.Id, target.Id }, cancellationToken)
                is { } userFollowing)
            {
                _context.UserFollowings.Remove(userFollowing);
            }
            else
            {
                _context.UserFollowings.Add(
                    new UserFollowing
                    {
                        Observer = observer,
                        Target = target
                    });
            }
            
            var noChangesWereMade = await _context.SaveChangesAsync(cancellationToken) == 0;

            if (noChangesWereMade) return new Error("Failed to update user following");

            return Unit.Value;
        }
    }
}