using System.Linq;
using App.DAL.Contracts.Repositories;
using AutoMapper;
using Base.DAL.EF;
using DAL.App.EF.Mappers;
using Microsoft.EntityFrameworkCore;
using AppDalDTO = App.DAL.DTO;
using AppDomain = App.Domain;



namespace DAL.App.EF.Repositories;

public class AnnouncementRepository : BaseEntityRepository<AppDalDTO.Announcement, AppDomain.Announcement, AppDbContext>, IAnnouncementRepository
{
    public AnnouncementRepository(AppDbContext dbContext, IMapper mapper ) : base(dbContext, new AnnouncementMapper(mapper))
    {
        
    }

    public async Task<IEnumerable<AppDalDTO.Announcement>> GetAllPlayerAnnouncementsByUserId(Guid playerId, bool noTracking = true)
    {
        var query = CreateQuery(default, noTracking);
            
        var announcements = await query.Include(x => x.AppUser)
            .Include(x => x.Team).ThenInclude(x => x!.PersonInTeams!)
            .Where(ann => ann.Team!.PersonInTeams!.Any(personInTeam => personInTeam.AppUserId.Equals(playerId)))
            .ToListAsync();

        return (announcements.Select(x => Mapper.Map(x))!);
    }
    
    public async Task<IEnumerable<AppDalDTO.Announcement>> GetAllAnnouncementsByClubId(IEnumerable<AppDalDTO.PersonInClub> playerClubIds)
    {
        var allAnnouncements = (await GetAllAsync()).ToList();

        var res = allAnnouncements.Where(x =>
            playerClubIds.Any(c => x.AppUser!.PersonInClubs!.Any(g => c.ClubId == g.ClubId)))
            .Where(x => x.TeamId == null);

        return res;
    }
    
    public override async Task<IEnumerable<AppDalDTO.Announcement>> GetAllAsync(Guid userId = default, bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);
        return await query.Include(x => x.Team)
            .Include(x => x.AppUser).ThenInclude(x => x!.PersonInClubs)
            .Select(x => Mapper.Map(x)!).ToListAsync();
    }

    public override async Task<AppDalDTO.Announcement?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);
        return Mapper.Map(await query.Include(x => x.AppUser)
            .Include(x => x.Team)
            .FirstOrDefaultAsync(x => x.Id == id));
    }
}