using System.Linq;
using Application.Core;
using Application.Interfaces;
using Domain;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class UpdateAttendance
{
    public record Command(Guid Id) : IRequestWrapper;

    public class Handler : IRequestHandlerWrapper<Command>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
        public Handler(DataContext dataContext, IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
            _context = dataContext;
        }

        public async Task<Either<Error, MediatR.Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await _context.Activities
                .Include(a => a.Attendees)
                .ThenInclude(u => u.AppUser)
                .SingleOrDefaultAsync(x => x.Id == request.Id)
                is not { } activity)
            {
                return default;
            }

            if (await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername())
                is not { } user)
            {
                return default;
            }

            var hostUsername = activity.Attendees.FirstOrDefault(x => x.IsHost)?.AppUser?.UserName;

            var attendance = activity.Attendees.FirstOrDefault(x => x.AppUser.UserName == user.UserName);

            if (attendance is null)
            {
                activity.Attendees.Add(
                    new ActivityAttendee
                    {
                        AppUser = user,
                        Activity = activity,
                        IsHost = false
                    });
            }
            else
            {
                if (hostUsername == user.UserName)
                {
                    activity.IsCancelled = !activity.IsCancelled;
                }
                else
                {
                    activity.Attendees.Remove(attendance);
                }
            }

            var noChangesWereMade = await _context.SaveChangesAsync(cancellationToken) == 0;

            if (noChangesWereMade) return new Error("Failed to update attendance");

            return MediatR.Unit.Value;
        }
    }
}