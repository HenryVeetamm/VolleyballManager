using AppDalDTO = App.DAL.DTO;
using Base.Contracts.DAL;
using Base.Contracts.Domain;

namespace App.DAL.Contracts.Repositories;

public interface IUserRepository : IEntityRepository<AppDalDTO.Identity.AppUser>,
    IUserRepositoryCustom<AppDalDTO.Identity.AppUser>
{
    
    
}

public interface IUserRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllClubPlayersByCoachClubId(IEnumerable<App.DAL.DTO.PersonInClub> coachId, bool noTracking = true);

}