using App.BLL.Contracts.Services;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.Contracts.Repositories;
using AutoMapper;
using Base.BLL.Services;
using AppBllDTO = App.BLL.DTO;
using AppDalDTO = App.DAL.DTO;

namespace App.BLL.Services;

public class PersonInTeamService: BaseEntityService<IAppUnitOfWork, IPersonInTeamRepository, AppBllDTO.PersonInTeam, AppDalDTO.PersonInTeam> 
    , IPersonInTeamService
{
    public PersonInTeamService(IAppUnitOfWork serviceUow, IPersonInTeamRepository serviceRepository, IMapper mapper)
        : base(serviceUow, serviceRepository, new PersonInTeamMapper(mapper))
    {
    }

    public async Task<IEnumerable<AppBllDTO.PersonInTeam>> GetAllPersonInTeamByTeamId(Guid personInTeamId, bool noTracking = true)
    {
        return (await ServiceUow.PersonInTeam.GetAllPersonInTeamByTeamId(personInTeamId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}