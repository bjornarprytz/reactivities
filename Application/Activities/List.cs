using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class List
{
    public record Query : IRequest<IEnumerable<Reactivity>>;
    public class Handler : IRequestHandler<Query, IEnumerable<Reactivity>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Reactivity>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Activities.ToListAsync(cancellationToken);
        }
    }
}