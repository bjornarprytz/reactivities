using Domain;
using MediatR;
using Persistence;

namespace Application.Activities;

public class Details
{
    public record Query(Guid Id) : IRequest<Reactivity?>;
    
    public class Handler : IRequestHandler<Query, Reactivity?>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Reactivity?> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Activities.FindAsync(new object?[] { request.Id }, cancellationToken: cancellationToken);
        }
    }
}