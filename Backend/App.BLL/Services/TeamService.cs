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

public class TeamService : BaseEntityService<IAppUnitOfWork, ITeamRepository, AppBllDTO.Team, AppDalDTO.Team> 
    , ITeamService
{
    public TeamService(IAppUnitOfWork serviceUow, ITeamRepository serviceRepository, IMapper mapper) 
        : base(serviceUow, serviceRepository, new TeamMapper(mapper))
    {
    }
    

    public async Task<IEnumerable<AppBllDTO.Team?>> GetAllPersonTeamsByUserId(Guid userId = default, bool noTracking = true)
    {
        return (await ServiceUow.Team.GetAllPersonTeamsByUserId(userId, noTracking)).Select(x => Mapper.Map(x));
    }

    public async Task<IEnumerable<AppBllDTO.Team?>> GetAllOpponentTeams(Guid userId = default, bool noTracking = true)
    {
        return (await ServiceUow.Team.GetAllOpponentTeams(userId, noTracking)).Select(x => Mapper.Map(x));
    }
    
}