using App.DAL.Contracts.Repositories;
using AutoMapper;
using AppDalDTO = App.DAL.DTO;
using AppDomain = App.Domain;
using Base.DAL.EF;
using DAL.App.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories;

public class PersonInMatchRepository :
    BaseEntityRepository<AppDalDTO.PersonInMatch, AppDomain.PersonInMatch, AppDbContext>, IPersonInMatchRepository
{
    public PersonInMatchRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext,
        new PersonInMatchMapper(mapper))
    {
    }

    /*public override async Task<IEnumerable<AppDalDTO.PersonInMatch>> GetAllAsync(Guid userId = default,
        bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);

        var queryAll = CreateQuery();

        var allList = await queryAll.Include(x => x.AppUser)
            .Include(x => x.Match)
            .Where(game => query.Any(g => g.MatchId == game.MatchId)).ToListAsync();

        return allList.Select(x => Mapper.Map(x))!;
    }*/

    public override async Task<AppDalDTO.PersonInMatch?> FirstOrDefaultAsync(Guid id, Guid userId = default,
        bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);
        
        return Mapper.Map(await query.Include(x => x.AppUser)
            .Include(x => x.Match)
            .FirstOrDefaultAsync(x => x.Id == id));
    }

    public async Task<IEnumerable<AppDalDTO.PersonInMatch>> GetUserPersonInMatches(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);
        return await query.Include(x => x.AppUser)
            .Include(x => x.Match).ThenInclude(x => x!.HomeTeam)
            .Include(x => x.Match).ThenInclude(x => x!.AwayTeam)
            .Select(x => Mapper.Map(x)!).ToListAsync();
    }

    public async Task<IEnumerable<AppDalDTO.PersonInMatch>> GetAllPersonInMatchByMatchId(Guid matchId, 
        bool noTracking = true)
    {
        var query = CreateQuery(default, noTracking);
        return await query.Include(x => x.AppUser)
            .Include(x => x.Match).Where(x => x.MatchId == matchId)
            .Select(x => Mapper.Map(x)!)
            .ToListAsync();
    }
}