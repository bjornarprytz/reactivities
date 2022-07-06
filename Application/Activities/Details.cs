using Application.Core;
using Domain;
using LanguageExt;
using MediatR;
using Persistence;

namespace Application.Activities;

public class Details
{
    public record Query(Guid Id) : IRequestWrapper<Reactivity?>;
    
    public class Handler : IRequestHandlerWrapper<Query, Reactivity?>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Either<Error, Reactivity?>> Handle(Query request, CancellationToken cancellationToken)
        {

            if (await _context.Activities.FindAsync(new object?[] { request.Id }, cancellationToken: cancellationToken)
                is not { } activity)
            {
                return default;
            }
            
            return activity;
        }
    }
}