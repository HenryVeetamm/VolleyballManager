
using AppDalDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.DAL.Contracts.Repositories;

public interface IPersonInClubRepository : IEntityRepository<AppDalDTO.PersonInClub>, 
    IPersonInClubRepositoryCustom<AppDalDTO.PersonInClub>
{
   
}

public interface IPersonInClubRepositoryCustom<TEntity>
{
    /*Task<IEnumerable<TEntity>> GetAllUserClubMembers(Guid userId, bool noTracking = true);*/

    Task<IEnumerable<TEntity>> GetAllUserClubs(Guid userId, bool noTracking = true);

    Task<IEnumerable<TEntity>> GetAllMembersOfClub(Guid clubId, bool noTracking = true);
}