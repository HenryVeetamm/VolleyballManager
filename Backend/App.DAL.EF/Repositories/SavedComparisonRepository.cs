using App.DAL.Contracts.Repositories;
using AutoMapper;
using AppDalDTO = App.DAL.DTO;
using AppDomain = App.Domain;
using Base.DAL.EF;
using DAL.App.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories;

public class SavedComparisonRepository : BaseEntityRepository<AppDalDTO.SavedComparison,AppDomain.SavedComparison, AppDbContext>,
    ISavedComparisonRepository
{
    public SavedComparisonRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new SavedComparisonMapper(mapper))
    {
    }

    public async Task<IEnumerable<AppDalDTO.SavedComparison>> GetPlayerComparisonsByUserId(Guid playerId, bool noTracking = true)
    {
        var query = CreateQuery(default, noTracking);

        return await query.Include(x => x.Comparer)
            .Include(x => x.Comparable)
            .Where(x => x.ComparerId == playerId)
            .Select(x => Mapper.Map(x)!).ToListAsync();
    }
    

    public override async Task<AppDalDTO.SavedComparison?> FirstOrDefaultAsync(Guid comparisonId, Guid userId = default, bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);

        return Mapper.Map(await query
            .FirstOrDefaultAsync(x=> x.Id == comparisonId));
    }
    
}