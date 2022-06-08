using App.DAL.Contracts.Repositories;
using AppBllDTO = App.BLL.DTO;
using AppDalDTO = App.DAL.DTO;
using Base.Contracts.BLL.Services;

namespace App.BLL.Contracts.Services;

public interface IPersonInMatchService : IBaseEntityService<AppBllDTO.PersonInMatch, AppDalDTO.PersonInMatch>,
    IPersonInMatchRepositoryCustom<AppBllDTO.PersonInMatch>
{
    Task<IEnumerable<AppBllDTO.PersonInMatch>> GetAllDetailedPersonInMatch(Guid userId, bool noTracking = true);
}