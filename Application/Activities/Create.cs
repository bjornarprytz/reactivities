using Domain;
using MediatR;
using Persistence;

namespace Application.Activities;

public class Create
{
    public record Command(Reactivity Reactivity) : IRequest;
    
    public class Handler : IRequestHandler<Command>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            _context.Activities.Add(request.Reactivity);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}