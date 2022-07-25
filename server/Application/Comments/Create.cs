using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments;

public class Create
{
    public record Command(Guid ActivityId, string Body) : IRequestWrapper<CommentDto>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Body).NotEmpty();
        }
    }

    public class Handler : IRequestHandlerWrapper<Command, CommentDto>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
        {
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }
        
        public async Task<Either<Error, CommentDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await _context.Activities.FindAsync(new object?[] { request.ActivityId }, cancellationToken)
                is not { } activity
                ||
                await _context.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken)
                is not { } user)
            {
                return default;
            }

            var comment = new Comment
            {
                Author = user,
                Reactivity = activity,
                Body = request.Body
            };
            
            activity.Comments.Add(comment);
            
            var noChangesWereMade = await _context.SaveChangesAsync(cancellationToken) == 0;

            if (noChangesWereMade) return new Error("Failed to add comment");

            return _mapper.Map<CommentDto>(comment);
        }
    }
}