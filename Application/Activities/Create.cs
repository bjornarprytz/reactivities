using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Unit = MediatR.Unit;

namespace Application.Activities;

public class Create
{
    public record Command(Reactivity Reactivity) : IRequestWrapper<Guid>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(a => a.Reactivity).SetValidator(new ActivityValidator());
        }
    }

    public class Handler : IRequestHandlerWrapper<Command, Guid>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Either<Error, Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var username = _userAccessor.GetUsername();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username, cancellationToken);

            if (user is null) return new Error("Failed to find user");

            var attendee = new ActivityAttendee
            {
                AppUser = user,
                Activity = request.Reactivity,
                IsHost = true
            };

            request.Reactivity.Attendees!.Add(attendee);
            _context.Activities.Add(request.Reactivity);

            var noChangesWereMade = await _context.SaveChangesAsync(cancellationToken) == 0;

            if (noChangesWereMade) return new Error("Failed to create activity");

            return request.Reactivity.Id;
        }
    }
}