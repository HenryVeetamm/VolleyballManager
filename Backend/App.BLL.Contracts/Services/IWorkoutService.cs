using App.DAL.Contracts.Repositories;
using AppBllDTO = App.BLL.DTO;
using AppDalDTO = App.DAL.DTO;
using Base.Contracts.BLL.Services;

namespace App.BLL.Contracts.Services;

public interface IWorkoutService: IBaseEntityService<AppBllDTO.Workout, AppDalDTO.Workout>,
    IWorkoutRepositoryCustom<AppBllDTO.Workout>
{
    
}