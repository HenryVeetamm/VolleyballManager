using AppDalDto = App.DAL.DTO;
using Base.Contracts.DAL;


namespace App.DAL.Contracts.Repositories;


public interface IAnnouncementRepository : IEntityRepository<AppDalDto.Announcement>, 
    IAnnouncementRepositoryCustom<AppDalDto.Announcement>
{
    
}

public interface IAnnouncementRepositoryCustom<TEntity>
{

    Task<IEnumerable<TEntity>>GetAllPlayerAnnouncementsByUserId(Guid playerId, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllAnnouncementsByClubId(IEnumerable<AppDalDto.PersonInClub> playerClubIds);
}