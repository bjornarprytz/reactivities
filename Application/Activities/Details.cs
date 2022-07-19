using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class Details
{
    public record Query(Guid Id) : IRequestWrapper<ActivityDto>;

    public class Handler : IRequestHandlerWrapper<Query, ActivityDto>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Either<Error, ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (await _context.Activities.ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUsername() })
                .FirstOrDefaultAsync(x => x.Id == request.Id)
                is not { } activity)
            {
                return default;
            }

            return activity;
        }
    }
}