using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles;

public class Details
{
    public record Query(string Username) : IRequestWrapper<Profile>;

    public class Handler : IRequestHandlerWrapper<Query, Profile>
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

        public async Task<Either<Error, Profile>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (await _context.Users
                    .ProjectTo<Profile>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUsername() })
                    .SingleOrDefaultAsync(x => x.Username == request.Username, cancellationToken)
                is not { } user)
            {
                return default;
            }

            return user;
        }
    }
}