using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments;

public class List
{
    public record Query(Guid ActivityId) : IRequestWrapper<List<CommentDto>>;
    
    public class Handler : IRequestHandlerWrapper<Query, List<CommentDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<Either<Error, List<CommentDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var comments = await _context.Comments
                .Where(x => x.Reactivity.Id == request.ActivityId)
                .OrderByDescending(x => x.CreatedAt)
                .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return comments;
        }
    }
}