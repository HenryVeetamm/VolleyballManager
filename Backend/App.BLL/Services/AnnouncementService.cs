using App.BLL.Contracts.Services;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.Contracts.Repositories;
using AutoMapper;
using AppBllDTO = App.BLL.DTO;
using AppDalDTO = App.DAL.DTO;
using Base.BLL.Services;

namespace App.BLL.Services;

public class AnnouncementService : BaseEntityService<IAppUnitOfWork, IAnnouncementRepository, AppBllDTO.Announcement, AppDalDTO.Announcement>, 
    IAnnouncementService
{
    public AnnouncementService(IAppUnitOfWork serviceUow, IAnnouncementRepository serviceRepository, IMapper mapper) 
        : base(serviceUow, serviceRepository, new AnnouncementMapper(mapper))
    {
    }

    public async Task<IEnumerable<AppBllDTO.Announcement>> GetAllPlayerAnnouncementsByUserId(Guid playerId, bool noTracking = true)
    {
        return (await ServiceRepository.GetAllPlayerAnnouncementsByUserId(playerId, noTracking)).Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<AppBllDTO.Announcement>> GetAllPlayerAnnouncements(Guid playerId)
    {
        var playerClubIds = (await ServiceUow.PersonInClub.GetAllUserClubs(playerId)).ToList();

        var allAnnouncements = (await ServiceRepository.GetAllAnnouncementsByClubId(playerClubIds))
            .ToList();

        var annons = await ServiceRepository.GetAllPlayerAnnouncementsByUserId(playerId);
   
        annons = annons.Concat(allAnnouncements).ToList();
        
        return annons.Select(x => Mapper.Map(x)!);
    }
    
    public async Task<IEnumerable<AppBllDTO.Announcement>> GetAllAnnouncementsByClubId(IEnumerable<AppDalDTO.PersonInClub> playerClubIds)
    {
        return (await ServiceRepository.GetAllAnnouncementsByClubId(playerClubIds)).Select(x => Mapper.Map(x)!);
    }
}