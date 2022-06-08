using AppDalDTO = App.DAL.DTO;
using Base.Contracts.DAL;
using PersonInClub = App.Resources.App.Domain.PersonInClub;

namespace App.DAL.Contracts.Repositories;

public interface IPersonInTeamRepository : IEntityRepository<AppDalDTO.PersonInTeam>, IPersonInTeamRepositoryCustom<AppDalDTO.PersonInTeam>
{
 
}

public interface IPersonInTeamRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllPersonInTeamByTeamId(Guid personInTeamId, bool noTracking = true);
}