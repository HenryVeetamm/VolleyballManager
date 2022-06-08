using App.DAL.Contracts.Repositories;
using AutoMapper;
using AppDalDto = App.DAL.DTO;
using AppDomain = App.Domain;
using Base.DAL.EF;
using DAL.App.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories;

public class MatchRepository : BaseEntityRepository<AppDalDto.Match, AppDomain.Match, AppDbContext>, IMatchRepository
{
    public MatchRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new MatchMapper(mapper))
    {
    }

    public override async Task<IEnumerable<AppDalDto.Match>> GetAllAsync(Guid userId = default, bool noTracking = true)
    {
        var query = CreateQuery(userId);

        return (await query.Include(x => x.AwayTeam).ThenInclude(x => x!.Club)
            .Include(x => x.HomeTeam).ThenInclude(x => x!.Club).ToListAsync()).Select(x => Mapper.Map(x)!);
    }

    public override async Task<AppDalDto.Match?> FirstOrDefaultAsync(Guid matchId, Guid userId = default, bool noTracking = true)
    {
        var query = CreateQuery(userId);
        
        return Mapper.Map(await query.Include(x => x.AwayTeam)
            .ThenInclude(x => x!.Club)
            .Include(x => x.HomeTeam)
            .ThenInclude(x => x!.Club)
            .FirstOrDefaultAsync(x => x.Id == matchId));
    }
}