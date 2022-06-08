using AppDalDto = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.DAL.Contracts.Repositories;

public interface IClubRepository : IEntityRepository<AppDalDto.Club>, 
    IClubRepositoryCustom<AppDalDto.Club>
{
  
}

public interface IClubRepositoryCustom<TEntity>
{
    // Task<IEnumerable<TEntity>> GetUserClubs(Guid userId, bool noTracking = true);

    Task<IEnumerable<TEntity>> GetUserOwnedClubs(Guid userId, bool noTracking = true);

    Task<IEnumerable<TEntity>> GetUserOpponentClubs(Guid userId, bool noTracking = true);
}