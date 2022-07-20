using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class List
{
    public record Query(ActivityParams Params) : IRequestWrapper<PagedList<ActivityDto>>;
    public class Handler : IRequestHandlerWrapper<Query, PagedList<ActivityDto>>
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

        public async Task<Either<Error, PagedList<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Activities
                .Where(d => d.Date >= request.Params.StartDate)
                .OrderBy(d => d.Date)
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUsername() })
                .AsQueryable();

            if (request.Params.IsGoing && !request.Params.IsHost)
            {
                query = query.Where(x => x.Attendees.Any(a => a.Username == _userAccessor.GetUsername()));
            }

            if (request.Params.IsHost && !request.Params.IsGoing)
            {
                query = query.Where(x => x.HostUsername == _userAccessor.GetUsername());
            }

            return await query.ToPagedListAsync(request.Params);
        }
    }
}