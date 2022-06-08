using AutoMapper;

namespace App.BLL.DTO.MappingProfiles;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<BLL.DTO.Announcement, DAL.DTO.Announcement>().ReverseMap();
        CreateMap<BLL.DTO.Club, DAL.DTO.Club>().ReverseMap();
        CreateMap<BLL.DTO.Match, DAL.DTO.Match>().ReverseMap();
        CreateMap<BLL.DTO.PersonInClub, DAL.DTO.PersonInClub>().ReverseMap();
        CreateMap<BLL.DTO.PersonInMatch, DAL.DTO.PersonInMatch>().ReverseMap();
        CreateMap<BLL.DTO.PersonInTeam, DAL.DTO.PersonInTeam>().ReverseMap();
        CreateMap<BLL.DTO.PersonInWorkout, DAL.DTO.PersonInWorkout>().ReverseMap();
        CreateMap<BLL.DTO.RolesInTeam, DAL.DTO.RolesInTeam>().ReverseMap();
        CreateMap<BLL.DTO.SavedComparison, DAL.DTO.SavedComparison>().ReverseMap();
        CreateMap<BLL.DTO.Team, DAL.DTO.Team>().ReverseMap();
        CreateMap<BLL.DTO.Workout, DAL.DTO.Workout>().ReverseMap();
        CreateMap<BLL.DTO.WorkoutType, DAL.DTO.WorkoutType>().ReverseMap();
        CreateMap<BLL.DTO.Identity.AppUser, DAL.DTO.Identity.AppUser>().ReverseMap();
        CreateMap<BLL.DTO.Identity.AppRole, DAL.DTO.Identity.AppRole>().ReverseMap();
    }
}