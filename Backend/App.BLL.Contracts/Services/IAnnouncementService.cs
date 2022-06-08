using AppBllDTO = App.BLL.DTO;
using AppDalDTO = App.DAL.DTO;
using App.DAL.Contracts.Repositories;

using Base.Contracts.BLL.Services;

namespace App.BLL.Contracts.Services;

public interface IAnnouncementService : IBaseEntityService<AppBllDTO.Announcement, AppDalDTO.Announcement>,
    IAnnouncementRepositoryCustom<AppBllDTO.Announcement>
{
    Task<IEnumerable<AppBllDTO.Announcement>> GetAllPlayerAnnouncements(Guid playerId);
}