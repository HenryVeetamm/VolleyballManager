using App.BLL.Contracts.Services;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.Contracts.Repositories;
using AutoMapper;
using Base.BLL.Services;
using Base.Contracts.BLL.Mappers;
using AppBllDTO = App.BLL.DTO;
using AppDalDTO = App.DAL.DTO;
namespace App.BLL.Services;

public class WorkoutService : BaseEntityService<IAppUnitOfWork, IWorkoutRepository, AppBllDTO.Workout, AppDalDTO.Workout> 
    , IWorkoutService
{
    public WorkoutService(IAppUnitOfWork serviceUow, IWorkoutRepository serviceRepository, IMapper mapper) 
        : base(serviceUow, serviceRepository, new WorkoutMapper(mapper))
    {
    }
}