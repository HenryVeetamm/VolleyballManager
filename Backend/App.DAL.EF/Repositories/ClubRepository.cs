using App.DAL.Contracts.Repositories;
using AutoMapper;
using AppDalDTO = App.DAL.DTO;
using AppDomain = App.Domain;
using Base.DAL.EF;
using DAL.App.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories;

public class ClubRepository : BaseEntityRepository<AppDalDTO.Club, AppDomain.Club, AppDbContext>, IClubRepository
{
    public ClubRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new ClubMapper(mapper))
    {
        
    }

    /*public async Task<IEnumerable<AppDalDTO.Club>> GetUserClubs(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery();

        var res = query.Include(x => x.PersonInClubs)
            .Where(x => x.PersonInClubs!.Any(x => x.AppUserId == userId));

        return (await res.ToListAsync()).Select(x => Mapper.Map(x)!);
    }*/

    public async Task<IEnumerable<AppDalDTO.Club>> GetUserOwnedClubs(Guid userId, bool noTracking = true)
    {
        var query =  CreateQuery(userId, noTracking);

        return await query.Where(x => x.OwnClub == true).Select(x => Mapper.Map(x)!).ToListAsync();
    }

    public async Task<IEnumerable<AppDalDTO.Club>> GetUserOpponentClubs(Guid userId, bool noTracking = true)
    {
        var query =  CreateQuery(userId, noTracking);

        return await query.Where(x => x.OwnClub == false).Select(x => Mapper.Map(x)!).ToListAsync();
    }
}