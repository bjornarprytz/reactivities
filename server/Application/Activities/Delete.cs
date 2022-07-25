using Application.Core;
using LanguageExt;
using Persistence;
using Unit = MediatR.Unit;

namespace Application.Activities;

public class Delete
{
    public record Command(Guid Id) : IRequestWrapper;
    
    public class Handler : IRequestHandlerWrapper<Command>
    {
        private readonly DataContext _context;

        public Handler(DataContext dataContext)
        {
            _context = dataContext;
        }
        
        public async Task<Either<Error, Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await _context.Activities.FindAsync(new object?[] { request.Id }, cancellationToken: cancellationToken)
                is not { } activity)
            {
                return default;
            }
            
            _context.Activities.Remove(activity);

            var noChangesWereMade = await _context.SaveChangesAsync(cancellationToken) == 0;

            if (noChangesWereMade) return new Error("Failed to delete the activity");
            
            return Unit.Value;
        }
    }
}