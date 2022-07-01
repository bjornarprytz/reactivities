using MediatR;
using Persistence;

namespace Application.Activities;

public class Delete
{
    public record Command(Guid Id) : IRequest;
    
    public class Handler : IRequestHandler<Command>
    {
        private readonly DataContext _dataContext;

        public Handler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var reactivity  = await _dataContext.Activities.FindAsync(new object?[] { request.Id }, cancellationToken: cancellationToken);

            if (reactivity is null)
            {
                return Unit.Value;
            }
            
            _dataContext.Activities.Remove(reactivity);

            await _dataContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}