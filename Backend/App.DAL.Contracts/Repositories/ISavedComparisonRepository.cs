using AppDalDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.DAL.Contracts.Repositories;

public interface ISavedComparisonRepository : IEntityRepository<AppDalDTO.SavedComparison>, 
    ISavedComparisonRepositoryCustom<AppDalDTO.SavedComparison>
{
    
}

public interface ISavedComparisonRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetPlayerComparisonsByUserId(Guid playerId, bool noTracking = true);
    
}