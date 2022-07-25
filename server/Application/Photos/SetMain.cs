using Application.Core;
using Application.Interfaces;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Unit = MediatR.Unit;

namespace Application.Photos;

public class SetMain
{
    public record Command(string Id) : IRequestWrapper;
    
    public class Handler : IRequestHandlerWrapper<Command>
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
        
        
        public async Task<Either<Error, Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            if(await _context.Users.Include(p => p.Photos)
                       .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken)
                   is not { Photos: {} photos } user
               || 
               photos.FirstOrDefault(x => x.Id == request.Id)
                   is not {} photo
              )
            {
                return default;
            }

            if (user.Photos.FirstOrDefault(x => x.IsMain) 
                is {} mainPhoto)
            {
                mainPhoto.IsMain = false;
            }

            photo.IsMain = true;
            
            var noChangesWereMade = await _context.SaveChangesAsync(cancellationToken) == 0;

            if (noChangesWereMade) return new Error("Failed set main photo");
            
            return Unit.Value;
        }
    }
}