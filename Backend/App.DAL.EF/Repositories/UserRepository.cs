using System.Linq;
using App.DAL.Contracts.Repositories;
using AppDalDTO = App.DAL.DTO;
using AppDomain = App.Domain;
using App.Domain.Identity;
using AutoMapper;
using Base.DAL.EF;
using Base.Domain;
using DAL.App.EF.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AppUser = App.DAL.DTO.Identity.AppUser;

namespace DAL.App.EF.Repositories;

public class UserRepository : BaseEntityRepository<AppDalDTO.Identity.AppUser,AppDomain.Identity.AppUser, AppDbContext>, IUserRepository
{
    

    public UserRepository(AppDbContext dbContext,IMapper mapper) : base(dbContext, new UserMapper(mapper))
    {
        
    }

    
    public async Task<IEnumerable<AppDalDTO.Identity.AppUser>> 
        GetAllClubPlayersByCoachClubId(IEnumerable<AppDalDTO.PersonInClub> coachClubId, bool noTracking = true)
    {
        
        var query = CreateQuery(default, noTracking);
        
        return (await query.Include(x => x.PersonInClubs).ToListAsync())
            .Where(player => 
                player.PersonInClubs!.Any(x=>coachClubId.Any(y => x.ClubId == y.ClubId)))
                    .Select(x => Mapper.Map(x)!);
    }

}