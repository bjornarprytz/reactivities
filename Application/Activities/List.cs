using Application.Core;
using Domain;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class List
{
    public record Query : IRequestWrapper<IEnumerable<Reactivity>>;
    public class Handler : IRequestHandlerWrapper<Query, IEnumerable<Reactivity>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Either<Error, IEnumerable<Reactivity>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Activities.ToListAsync(cancellationToken);
        }
    }
}