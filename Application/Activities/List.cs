using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class List
{
    public record Query : IRequestWrapper<IEnumerable<ActivityDto>>;
    public class Handler : IRequestHandlerWrapper<Query, IEnumerable<ActivityDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Either<Error, IEnumerable<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activities = await _context.Activities
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return activities;
        }
    }
}