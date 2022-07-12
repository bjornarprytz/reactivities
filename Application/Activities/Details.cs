using Application.Core;
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

        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Either<Error, ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (await _context.Activities.ProjectTo<ActivityDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(x => x.Id == request.Id)
            is not { } activity)
            {
                return default;
            }

            return activity;
        }
    }
}