using System.Linq;
using App.DAL.Contracts.Repositories;
using AutoMapper;
using AppDalDTO = App.DAL.DTO;
using AppDomain = App.Domain;
using Base.DAL.EF;
using DAL.App.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories;

public class PersonInWorkoutRepository : BaseEntityRepository<AppDalDTO.PersonInWorkout, AppDomain.PersonInWorkout, AppDbContext>,
    IPersonInWorkoutRepository
{
    public PersonInWorkoutRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new PersonInWorkoutMapper(mapper))
    {
    }

    public override async Task<IEnumerable<AppDalDTO.PersonInWorkout>> GetAllAsync(Guid userId = default, bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);

        return await query
            .Include(x => x.AppUser)
            .Include(x => x.Workout)
            .ThenInclude(x => x!.WorkoutType)
            .Select(x => Mapper.Map(x)!)
            .ToListAsync();
    }

    public async Task<IEnumerable<AppDalDTO.PersonInWorkout>> GetAllPersonInWorkoutByWorkoutId(Guid workoutId, bool noTracking = true)
    {
        var query = CreateQuery(default, noTracking);
        
        return await query
            .Include(x => x.Workout)
            .ThenInclude(x => x!.WorkoutType)
            .Include(x => x.AppUser)
            .Where(x => x.WorkOutId == workoutId )
            .Select(x => Mapper.Map(x)!)
            .ToListAsync();
    }

    //edited query
    public override async Task<AppDalDTO.PersonInWorkout?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);

        return Mapper.Map(await query
            .Include(x => x.AppUser)
            .Include(x => x.Workout).ThenInclude(x =>x!.WorkoutType)
            .FirstOrDefaultAsync(x => x.Id == id));
    }
    
}