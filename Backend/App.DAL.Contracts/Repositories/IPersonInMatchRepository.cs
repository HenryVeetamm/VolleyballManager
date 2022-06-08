using AppDalDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.DAL.Contracts.Repositories;

public interface IPersonInMatchRepository : IEntityRepository<AppDalDTO.PersonInMatch>,
    IPersonInMatchRepositoryCustom<AppDalDTO.PersonInMatch>
{
    
    
    
}

public interface IPersonInMatchRepositoryCustom<TEntity>
{

    Task<IEnumerable<TEntity>> GetUserPersonInMatches(Guid userId, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllPersonInMatchByMatchId(Guid matchId, bool noTracking = true);
}