using App.DAL.Contracts.Repositories;
using AutoMapper;
using AppDalDTO = App.DAL.DTO;
using AppDomain = App.Domain;

using Base.DAL.EF;
using DAL.App.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories;

public class PersonInClubRepository : BaseEntityRepository<AppDalDTO.PersonInClub,AppDomain.PersonInClub, AppDbContext>, IPersonInClubRepository
{
    public PersonInClubRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new PersonInClubMapper(mapper))
    {
    }

    /*public async Task<IEnumerable<AppDalDTO.PersonInClub>> GetAllUserClubMembers(Guid userId = default, bool noTracking = true)
    {
        var playerClubs = await CreateQuery(userId, noTracking).ToListAsync();
        var query = CreateQuery();

        var allMembers = await query.Include(x => x.Club)
            .Include(x => x.AppUser).ToListAsync();

        return allMembers.Where(x => playerClubs.Any(club => club.ClubId == x.ClubId)).Select(x => Mapper.Map(x))!;
    }*/

    public async  Task<IEnumerable<AppDalDTO.PersonInClub>> GetAllUserClubs(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);

        return (await query.Include(x => x.Club)
            .ToListAsync())
            .Select(x => Mapper.Map(x))!;
    }

    public async Task<IEnumerable<AppDalDTO.PersonInClub>> GetAllMembersOfClub(Guid clubId, bool noTracking = true)
    {
        var query = CreateQuery(default, noTracking);
        return await query.Include(x => x.AppUser)
            .Where(x => x.ClubId == clubId)
            .Select(x => Mapper.Map(x)!)
            .ToListAsync();
    }

    public override async Task<AppDalDTO.PersonInClub?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
    {
        
        /*var personClubs = (await GetAllUserClubMembers(userId, noTracking)).ToList();*/

        var query = CreateQuery(default, noTracking);
        
        return Mapper.Map(await query.Include(x => x.Club)
            .Include(x => x.AppUser).FirstOrDefaultAsync(x => x.Id == id));
        
        /*return personClubs.FirstOrDefault(x => x.Id == id);*/
    }
    
    

}