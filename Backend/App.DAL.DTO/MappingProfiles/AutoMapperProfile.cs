using AutoMapper;



namespace App.DAL.DTO.MappingProfiles;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<DAL.DTO.Announcement, Domain.Announcement>().ReverseMap();
        CreateMap<DAL.DTO.Club, Domain.Club>().ReverseMap();
        CreateMap<DAL.DTO.Match, Domain.Match>().ReverseMap();
        CreateMap<DAL.DTO.PersonInClub, Domain.PersonInClub>().ReverseMap();
        CreateMap<DAL.DTO.PersonInMatch, Domain.PersonInMatch>().ReverseMap();
        CreateMap<DAL.DTO.PersonInTeam, Domain.PersonInTeam>().ReverseMap();
        CreateMap<DAL.DTO.PersonInWorkout, Domain.PersonInWorkout>().ReverseMap();
        CreateMap<DAL.DTO.RolesInTeam, Domain.RolesInTeam>().ReverseMap();
        CreateMap<DAL.DTO.SavedComparison, Domain.SavedComparison>().ReverseMap();
        CreateMap<DAL.DTO.Team, Domain.Team>().ReverseMap();
        CreateMap<DAL.DTO.Workout, Domain.Workout>().ReverseMap();
        CreateMap<DAL.DTO.WorkoutType, Domain.WorkoutType>().ReverseMap();
        CreateMap<DAL.DTO.Identity.AppUser, Domain.Identity.AppUser>().ReverseMap();
        CreateMap<DAL.DTO.Identity.AppRole, Domain.Identity.AppRole>().ReverseMap();
    }
}