using AutoMapper;
using PublicDto = PublicAPI.DTO.v1;
using AppBllDto = App.BLL.DTO;

namespace PublicAPI.DTO.v1.MappingProfiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<PublicDto.Announcement, AppBllDto.Announcement>().ReverseMap();
        CreateMap<PublicDto.Club, AppBllDto.Club>().ReverseMap();
        CreateMap<PublicDto.Match, AppBllDto.Match>().ReverseMap();
        CreateMap<PublicDto.PersonInClub, AppBllDto.PersonInClub>().ReverseMap();
        CreateMap<PublicDto.PersonInMatch, AppBllDto.PersonInMatch>().ReverseMap();
        CreateMap<PublicDto.PersonInTeam, AppBllDto.PersonInTeam>().ReverseMap();
        CreateMap<PublicDto.PersonInWorkout, AppBllDto.PersonInWorkout>().ReverseMap();
        CreateMap<PublicDto.RolesInTeam, AppBllDto.RolesInTeam>().ReverseMap();
        CreateMap<PublicDto.SavedComparison, AppBllDto.SavedComparison>().ReverseMap();
        CreateMap<PublicDto.Team, AppBllDto.Team>().ReverseMap();
        CreateMap<PublicDto.Workout, AppBllDto.Workout>().ReverseMap();
        CreateMap<PublicDto.WorkoutType, AppBllDto.WorkoutType>().ReverseMap();
        CreateMap<PublicDto.Identity.AppUser, AppBllDto.Identity.AppUser>().ReverseMap();
        CreateMap<PublicDto.Identity.AppRole, AppBllDto.Identity.AppRole>().ReverseMap();
        
        
        CreateMap<PublicDto.SavedComparisonDetailed, AppBllDto.SavedComparisonDetailed>().ReverseMap();
        CreateMap<PublicDto.Identity.AppUserSimple, AppBllDto.Identity.AppUserSimple>().ReverseMap();
        CreateMap<PublicDto.Identity.AppUserSimple, AppBllDto.Identity.AppUser>().ReverseMap();
        CreateMap<PublicDto.MatchUpdate, AppBllDto.Match>().ReverseMap();
        CreateMap<PublicDto.AnnouncementPost, AppBllDto.Announcement>().ReverseMap();

        CreateMap<App.Domain.Identity.AppUser, PublicAPI.DTO.v1.Identity.AppUser>();

        CreateMap<PublicDto.TeamUpdate, AppBllDto.Team>();
    }
}