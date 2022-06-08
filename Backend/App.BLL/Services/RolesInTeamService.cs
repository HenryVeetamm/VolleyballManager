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

public class RolesInTeamService : BaseEntityService<IAppUnitOfWork, IRolesInTeamRepository, AppBllDTO.RolesInTeam, AppDalDTO.RolesInTeam> 
    , IRolesInTeamService
{
    public RolesInTeamService(IAppUnitOfWork serviceUow, IRolesInTeamRepository serviceRepository, IMapper mapper) 
        : base(serviceUow, serviceRepository, new RolesInTeamMapper(mapper))
    {
    }
}