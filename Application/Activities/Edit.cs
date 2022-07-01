using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities;

public class Edit
{
    public record Command(Reactivity Reactivity) : IRequest;
    
    public class Handler : IRequestHandler<Command>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var reactivity = await _context.Activities.FindAsync(new object?[] { request.Reactivity.Id }, cancellationToken: cancellationToken);

            _mapper.Map(request.Reactivity, reactivity);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}