using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles;

public class ListActivities
{
    public record Query(string Username, string Predicate) : IRequestWrapper<List<UserActivityDto>>;
    
    
    public class Handler : IRequestHandlerWrapper<Query, List<UserActivityDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<Either<Error, List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Activities
                .Where(a => a.Attendees.Any(u => u.AppUser.UserName == request.Username))
                .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            switch (request.Predicate)
            {
                case "past":
                    query = query.Where(a => a.Date < DateTime.UtcNow);
                    break;
                case "future":
                    query = query.Where(a => a.Date >= DateTime.UtcNow);
                    break;
                case "hosting":
                    query = query.Where(a => a.HostUsername == request.Username);
                    break;
                default:
                    return new Error($"Failure: Invalid predicate {request.Predicate}. Accepted predicates: [past|future|hosting]");
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}