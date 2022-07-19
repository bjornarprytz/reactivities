using Application.Activities;
using Application.Comments;
using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfiles : Profile
{
    
    public MappingProfiles()
    {
        string currentUsername = null; // This variable is set via named field of anonymous object (e.g. new { currentUsername = _userAccessor.GetUsername() })
        
        CreateMap<Reactivity, Reactivity>();
        CreateMap<Reactivity, ActivityDto>()
            .ForMember(
                d => d.HostUsername,
                o => o.MapFrom(s => s.Attendees.FirstOrDefault(x => x.IsHost).AppUser.UserName));

        CreateMap<ActivityAttendee, AttendeeDto>()
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
            .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
            .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio))
            .ForMember(d => d.Image, 
                o => o.MapFrom(s => s.AppUser.Photos!.FirstOrDefault(i => i.IsMain).Url))
            .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.AppUser.Followers.Count))
            .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.AppUser.Followings.Count))
            .ForMember(d => d.Following, o => o.MapFrom(s => s.AppUser.Followings.Any(x => x.Observer.UserName == currentUsername)))
            ;

        CreateMap<AppUser, Profiles.Profile>()
            .ForMember(d => d.Image, 
                o => o.MapFrom(s => s.Photos!.FirstOrDefault(i => i.IsMain).Url))
            .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
            .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count))
            .ForMember(d => d.Following, o => o.MapFrom(s => s.Followings.Any(x => x.Observer.UserName == currentUsername)))
            ;

        CreateMap<Comment, CommentDto>()
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.Author.DisplayName))
            .ForMember(d => d.Username, o => o.MapFrom(s => s.Author.UserName))
            .ForMember(d => d.Image,
                o => o.MapFrom(s => s.Author.Photos!.FirstOrDefault(i => i.IsMain).Url));
    }
}