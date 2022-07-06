using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using LanguageExt;
using Persistence;
using Unit = MediatR.Unit;

namespace Application.Activities;

public class Edit
{
    public record Command(Reactivity Reactivity) : IRequestWrapper<Unit?>;
    
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(a => a.Reactivity).SetValidator(new ActivityValidator());
        }
    }
    
    public class Handler : IRequestHandlerWrapper<Command, Unit?>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<Either<Error, Unit?>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await _context.Activities.FindAsync(new object?[] { request.Reactivity.Id }, cancellationToken: cancellationToken)
                is not { } activity)
            {
                return default;
            }

            _mapper.Map(request.Reactivity, activity);

            var noChangesWereMade = await _context.SaveChangesAsync(cancellationToken) == 0;

            if (noChangesWereMade) return new Error("Failed to update the activity");
            
            return Unit.Value;
        }
    }
}