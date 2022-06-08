using AppDalDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.DAL.Contracts.Repositories;

public interface IRolesInTeamRepository : IEntityRepository<AppDalDTO.RolesInTeam>,
    IRolesInTeamRepositoryCustom<AppDalDTO.RolesInTeam>
{
    
}

public interface IRolesInTeamRepositoryCustom<TEntity>
{
    
}