using App.DAL.Contracts.Repositories;
using AppBllDTO = App.BLL.DTO;
using AppDalDTO = App.DAL.DTO;
using Base.Contracts.BLL.Services;

namespace App.BLL.Contracts.Services;

public interface IMatchService :IBaseEntityService<AppBllDTO.Match, AppDalDTO.Match>,
    IMatchRepositoryCustom<AppBllDTO.Match>
{
    Task<IEnumerable<AppBllDTO.Match>> GetAllMatchesAsync(Guid userId = default, bool noTracking = true);
}