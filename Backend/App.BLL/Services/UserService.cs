using App.BLL.Contracts.Services;
using App.BLL.DTO.Identity;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.Contracts.Repositories;
using AutoMapper;
using Base.BLL.Services;
using Microsoft.AspNetCore.Identity;
using AppBllDTO = App.BLL.DTO;
using AppDalDTO = App.DAL.DTO;
namespace App.BLL.Services;

public class UserService : BaseEntityService<IAppUnitOfWork, IUserRepository, AppBllDTO.Identity.AppUser, AppDalDTO.Identity.AppUser> 
    , IUserService
{
    

    public UserService(IAppUnitOfWork serviceUow, IUserRepository serviceRepository, IMapper mapper) 
        : base(serviceUow, serviceRepository, new UserMapper(mapper))
    {
      
    }

    public async Task<IEnumerable<AppUser>> GetAllClubPlayersByCoachClubId(IEnumerable<AppDalDTO.PersonInClub> coachId, bool noTracking = true)
    {
        return (await ServiceUow.Users.GetAllClubPlayersByCoachClubId(coachId, noTracking)).Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<AppUser>> GetAllClubMembers(Guid userId, bool noTracking = true)
    {
        
        var coachClubIds = (await ServiceUow.PersonInClub.GetAllUserClubs(userId, noTracking));
        
        var clubPlayers = (await ServiceUow.Users.GetAllClubPlayersByCoachClubId(coachClubIds))
            .Select(x => Mapper.Map(x))
            .ToList();
        
        return clubPlayers!;
    }
}