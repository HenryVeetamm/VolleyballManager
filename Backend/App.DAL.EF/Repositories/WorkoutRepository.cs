using App.DAL.Contracts.Repositories;
using AutoMapper;
using AppDalDTO = App.DAL.DTO;
using AppDomain = App.Domain;
using Base.DAL.EF;
using DAL.App.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories;

public class WorkoutRepository: BaseEntityRepository<AppDalDTO.Workout,AppDomain.Workout, AppDbContext>, IWorkoutRepository
{
    public WorkoutRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new WorkoutMapper(mapper))
    {
    }

    public override async Task<IEnumerable<AppDalDTO.Workout>> GetAllAsync(Guid userId = default, bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);
        
        return await query
            .Include(x => x.WorkoutType)
            .Select(x => Mapper.Map(x)!)
            .ToListAsync();
    }

    public override async Task<AppDalDTO.Workout?> FirstOrDefaultAsync(Guid id, Guid userId = default
        , bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);
        return Mapper.Map(await query.
            Include(w => w.WorkoutType)
            .FirstOrDefaultAsync(x => x.Id.Equals(id)));
    }
    
}