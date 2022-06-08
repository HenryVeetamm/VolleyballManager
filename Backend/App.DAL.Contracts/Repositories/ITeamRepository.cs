using AppDalDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.DAL.Contracts.Repositories;

public interface ITeamRepository : IEntityRepository<AppDalDTO.Team>, 
    ITeamRepositoryCustom<AppDalDTO.Team>
{
   
}

public interface ITeamRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity?>> GetAllPersonTeamsByUserId(Guid userId = default!, bool noTracking = true);
    Task<IEnumerable<TEntity?>> GetAllOpponentTeams(Guid userId = default!, bool noTracking = true);
}