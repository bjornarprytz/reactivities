using Application.Core;
using Domain;
using FluentValidation;
using LanguageExt;
using Persistence;
using Unit = MediatR.Unit;

namespace Application.Activities;

public class Create
{
    public record Command(Reactivity Reactivity) : IRequestWrapper;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(a => a.Reactivity).SetValidator(new ActivityValidator());
        }
    }

    public class Handler : IRequestHandlerWrapper<Command>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Either<Error, Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            _context.Activities.Add(request.Reactivity);

            var noChangesWereMade = await _context.SaveChangesAsync(cancellationToken) == 0;
            
            if (noChangesWereMade) return new Error("Failed to create activity");
            
            return Unit.Value;
        }
    }
}