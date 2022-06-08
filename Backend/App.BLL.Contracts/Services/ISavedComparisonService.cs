using App.DAL.Contracts.Repositories;
using AppBllDTO = App.BLL.DTO;
using AppDalDTO = App.DAL.DTO;
using Base.Contracts.BLL.Services;

namespace App.BLL.Contracts.Services;

public interface ISavedComparisonService :IBaseEntityService<AppBllDTO.SavedComparison, AppDalDTO.SavedComparison>,
    ISavedComparisonRepositoryCustom<AppBllDTO.SavedComparison>
{
    Task<IEnumerable<AppBllDTO.SavedComparisonDetailed>> GetDetailedSavedComparison(Guid id, Guid userId, bool noTracking = true);
}