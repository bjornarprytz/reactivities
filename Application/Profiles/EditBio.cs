using Application.Core;
using Application.Interfaces;
using FluentValidation;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Unit = MediatR.Unit;

namespace Application.Profiles;

public class EditBio
{
    public record Command(string DisplayName, string? Bio) : IRequestWrapper;
    
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(a => a.DisplayName).NotEmpty();
        }
    }
    
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
            if(await _context.Users.FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername(), cancellationToken)
                   is not { } user)
            {
                return default;
            }

            user.DisplayName = request.DisplayName;
            user.Bio = request.Bio ?? user.Bio;
            
            var noChangesWereMade = await _context.SaveChangesAsync(cancellationToken) == 0;

            if (noChangesWereMade) return new Error("No changes were made");
            
            return Unit.Value;
        }
    }
}