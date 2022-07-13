using Application.Core;
using Application.Interfaces;
using Domain;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos;

public class Add
{
    public record Command(IFormFile File) : IRequestWrapper<Photo>;
    
    public class Handler : IRequestHandlerWrapper<Command, Photo>
    {
        private readonly DataContext _context;
        private readonly IPhotoAccessor _photoAccessor;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
        {
            _context = context;
            _photoAccessor = photoAccessor;
            _userAccessor = userAccessor;
        }
        
        public async Task<Either<Error, Photo>> Handle(Command request, CancellationToken cancellationToken)
        {
            if(await _context.Users.Include(p => p.Photos)
                       .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken)
               is not { Photos: {} } user
               || 
               await _photoAccessor.AddPhotoAsync(request.File, cancellationToken)
               is not {} photoUploadResult
               )
            {
                return default;
            }

            var photo = new Photo
            {
                Url = photoUploadResult.Url,
                Id = photoUploadResult.PublicId,
                IsMain = !user.Photos.Any(x => x.IsMain) 
            };
            
            user.Photos.Add(photo);

            var noChangesWereMade = await _context.SaveChangesAsync(cancellationToken) == 0;

            if (noChangesWereMade) return new Error("Failed to add photo");

            return photo;
        }
    }
}