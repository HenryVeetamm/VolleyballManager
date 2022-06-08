using AppDalDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.DAL.Contracts.Repositories;

public interface IWorkoutTypeRepository : IEntityRepository<AppDalDTO.WorkoutType>,
    IWorkoutTypeRepositoryCustom<AppDalDTO.WorkoutType>
{
    
}

public interface IWorkoutTypeRepositoryCustom<TEntity>
{
    
}