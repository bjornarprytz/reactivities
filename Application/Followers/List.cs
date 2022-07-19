using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Profile = Application.Profiles.Profile;

namespace Application.Followers;

public class List
{
    public record Query(string Predicate, string Username) : IRequestWrapper<List<Profile>>;

    public class Handler : IRequestHandlerWrapper<Query, List<Profile>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Either<Error, List<Profile>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return (request.Predicate) switch
            {
                "followers" => await _context.UserFollowings.Where(x => x.Target.UserName == request.Username)
                    .Select(u => u.Observer)
                    .ProjectTo<Profile>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUsername() })
                    .ToListAsync(cancellationToken),

                "following" => await _context.UserFollowings.Where(x => x.Observer.UserName == request.Username)
                    .Select(u => u.Target)
                    .ProjectTo<Profile>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUsername() })
                    .ToListAsync(cancellationToken),

                _ => new Error($"Invalid predicate {request.Predicate}. followers|following are valid predicates.")
            };
        }
    }
}