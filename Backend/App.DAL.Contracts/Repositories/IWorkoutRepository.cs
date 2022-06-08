using AppDalDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.DAL.Contracts.Repositories;

public interface IWorkoutRepository : IEntityRepository<AppDalDTO.Workout>,
    IWorkoutRepositoryCustom<AppDalDTO.Workout>
{

}

public interface IWorkoutRepositoryCustom<TEntity>
{
    

}