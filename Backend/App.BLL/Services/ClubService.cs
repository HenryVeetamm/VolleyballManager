using App.BLL.Contracts.Services;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.Contracts.Repositories;
using AutoMapper;
using Base.BLL.Services;
using Base.Contracts.BLL.Mappers;
using AppBllDTO = App.BLL.DTO;
using AppDalDTO = App.DAL.DTO;

namespace App.BLL.Services;

public class ClubService :  BaseEntityService<IAppUnitOfWork, IClubRepository, AppBllDTO.Club, AppDalDTO.Club>,
    IClubService
{
    public ClubService(IAppUnitOfWork serviceUow, IClubRepository serviceRepository, 
        IMapper mapper) 
        : base(serviceUow, serviceRepository, new ClubMapper(mapper))
    {
    }

    public async Task<IEnumerable<AppBllDTO.Club>> GetUserOwnedClubs(Guid userId, bool noTracking = true)
    {
        return (await ServiceRepository.GetUserOwnedClubs(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<AppBllDTO.Club>> GetUserOpponentClubs(Guid userId, bool noTracking = true)
    {
        return (await ServiceRepository.GetUserOpponentClubs(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}