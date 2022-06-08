using App.DAL.Contracts.Repositories;
using AutoMapper;
using AppDalDTO = App.DAL.DTO;
using AppDomain = App.Domain;
using Base.DAL.EF;
using DAL.App.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories;

public class TeamRepository : BaseEntityRepository<AppDalDTO.Team,AppDomain.Team, AppDbContext>, ITeamRepository
{
    public TeamRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new TeamMapper(mapper))
    {
    }

    public override async Task<AppDalDTO.Team?> FirstOrDefaultAsync(Guid teamId, Guid userId = default!, bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);

        return Mapper.Map(await query.Include(x => x.Club).FirstOrDefaultAsync(x => x.Id == teamId));
    }
    
    public async Task<IEnumerable<AppDalDTO.Team?>> GetAllPersonTeamsByUserId(Guid userId = default, bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);

        return await query
            .Include(x => x.Club).Where(x => x.OwnTeam == true)
            .Select(x => Mapper.Map(x)!)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<AppDalDTO.Team?>> GetAllOpponentTeams(Guid userId = default, bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);
        
        return await  query.Include(x=> x.Club).Where(x => x.OwnTeam == false)
            .Select(x => Mapper.Map(x)!)
            .ToListAsync();
    }
    
    public override async Task<IEnumerable<AppDalDTO.Team>> GetAllAsync(Guid userId = default ,bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);

        return await query
            .Include(x => x.Club)
            .Select(x => Mapper.Map(x)!)
            .ToListAsync();
    }
}