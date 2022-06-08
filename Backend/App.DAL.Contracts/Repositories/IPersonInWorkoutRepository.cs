using AppDalDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.DAL.Contracts.Repositories;

public interface IPersonInWorkoutRepository : IEntityRepository<AppDalDTO.PersonInWorkout>, 
    IPersonInWorkoutRepositoryCustom<AppDalDTO.PersonInWorkout>
{
    

}

public interface IPersonInWorkoutRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllPersonInWorkoutByWorkoutId(Guid workoutId, bool noTracking = true);

}