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

public class PersonInClubService: BaseEntityService<IAppUnitOfWork, IPersonInClubRepository, AppBllDTO.PersonInClub, AppDalDTO.PersonInClub> 
    , IPersonInClubService
{
    public PersonInClubService(IAppUnitOfWork serviceUow, IPersonInClubRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, 
        new PersonInClubMapper(mapper))
    {
    }

    public async Task<IEnumerable<AppBllDTO.PersonInClub>> GetAllUserClubs(Guid userId, bool noTracking = true)
    {
        return (await ServiceUow.PersonInClub.GetAllUserClubs(userId)).Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<AppBllDTO.PersonInClub>> GetAllMembersOfClub(Guid clubId, bool noTracking = true)
    {
        return (await ServiceUow.PersonInClub.GetAllMembersOfClub(clubId)).Select(x => Mapper.Map(x)!).ToList();
    }
}