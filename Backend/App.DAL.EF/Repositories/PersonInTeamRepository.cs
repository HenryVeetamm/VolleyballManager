using App.DAL.Contracts.Repositories;
using AutoMapper;
using AppDalDTO = App.DAL.DTO;
using AppDomain = App.Domain;
using Base.DAL.EF;
using DAL.App.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories;

public class PersonInTeamRepository :
    BaseEntityRepository<AppDalDTO.PersonInTeam, AppDomain.PersonInTeam, AppDbContext>, IPersonInTeamRepository
{
    public PersonInTeamRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext,
        new PersonInTeamMapper(mapper))
    {
    }


    public async Task<IEnumerable<AppDalDTO.PersonInTeam>> GetAllPersonInTeamByTeamId(Guid teamId,
        bool noTracking = true)
    {
        var query = CreateQuery(default, noTracking);

        var personInTeams = await query.Include(user => user.AppUser)
            .Include(role => role.RolesInTeam)
            .Include(team => team.Team).ThenInclude(x => x!.Club)
            .Where(x => x.TeamId == teamId).ToListAsync();

        return personInTeams.Select(x => Mapper.Map(x)!);
    }

    public override async Task<IEnumerable<AppDalDTO.PersonInTeam>> GetAllAsync(Guid userId = default,
        bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);

        return await query.Include(user => user.AppUser)
            .Include(role => role.RolesInTeam)
            .Include(team => team.Team).ThenInclude(club => club!.Club)
            .Select(x => Mapper.Map(x)!).ToListAsync();
    }

    public override async Task<AppDalDTO.PersonInTeam?> FirstOrDefaultAsync(Guid id, Guid userId = default,
        bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);

        return Mapper.Map(await query.Include(x => x.AppUser)
            .Include(x => x.Team)
            .Include(x => x.RolesInTeam).FirstOrDefaultAsync(x => x.Id == id));
    }
}