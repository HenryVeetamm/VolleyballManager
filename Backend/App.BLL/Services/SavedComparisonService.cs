using App.BLL.Contracts.Services;
using App.BLL.DTO.Identity;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.Contracts.Repositories;
using AutoMapper;
using Base.BLL.Services;
using Base.Contracts.BLL.Mappers;
using AppBllDTO = App.BLL.DTO;
using AppDalDTO = App.DAL.DTO;
namespace App.BLL.Services;

public class SavedComparisonService : BaseEntityService<IAppUnitOfWork, ISavedComparisonRepository, AppBllDTO.SavedComparison, AppDalDTO.SavedComparison> 
    , ISavedComparisonService
{
    public SavedComparisonService(IAppUnitOfWork serviceUow, ISavedComparisonRepository serviceRepository, IMapper mapper)
        : base(serviceUow, serviceRepository,  new SavedComparisonMapper(mapper))
    {
    }

    public async Task<IEnumerable<AppBllDTO.SavedComparison>> GetPlayerComparisonsByUserId(Guid playerId, bool noTracking = true)
    {
        return (await ServiceUow.SavedComparison.GetPlayerComparisonsByUserId(playerId, noTracking)).Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<AppBllDTO.SavedComparisonDetailed>> GetDetailedSavedComparison(Guid id, Guid userId, bool noTracking = true)
    {

        var savedComparison = await ServiceRepository.FirstOrDefaultAsync(id, userId, noTracking);

        var comparer = await ServiceUow.Users.FirstOrDefaultAsync(savedComparison!.ComparerId);
        
        var comparable = await ServiceUow.Users.FirstOrDefaultAsync(savedComparison!.ComparableId);

        var firstPersonMatches =
            (await ServiceUow.PersonInMatch.GetUserPersonInMatches(savedComparison!.ComparerId, noTracking)).ToList();

        var secondPersonMatches =
            (await ServiceUow.PersonInMatch.GetUserPersonInMatches(savedComparison!.ComparableId, noTracking)).ToList();

        var firstPersonDetailed = MakeDetailedData(firstPersonMatches, comparer!);
        var secondPersonDetailed = MakeDetailedData(secondPersonMatches, comparable!);

        
        var res = new List<AppBllDTO.SavedComparisonDetailed>() { firstPersonDetailed, secondPersonDetailed };
        
        return res;
    }

    private static AppBllDTO.SavedComparisonDetailed MakeDetailedData(List<AppDalDTO.PersonInMatch> personMatches, 
        AppDalDTO.Identity.AppUser user)
    {
        var personDetailed = new AppBllDTO.SavedComparisonDetailed();
        
        personDetailed.Comparer = new AppUserSimple()
        {
            FirstName = user.FirstName,
            LastName = user.LastName
        };
        personDetailed.Aces = 0;
        foreach (var entry in personMatches)
        {
            personDetailed.Aces += entry.Aces ?? 0;
            personDetailed.Faults += entry.Faults ?? 0;
            personDetailed.Points += entry.TotalPoints ?? 0;
            personDetailed.Reception += entry.Reception ?? 0;
            personDetailed.TotalMatches++;
        }

        personDetailed.Reception = personDetailed.TotalMatches != 0 ? personDetailed.Reception / personDetailed.TotalMatches :
                0;

        return personDetailed;
    }
    
}