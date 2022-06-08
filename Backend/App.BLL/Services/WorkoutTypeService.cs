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

public class WorkoutTypeService : 
    BaseEntityService<IAppUnitOfWork, IWorkoutTypeRepository, AppBllDTO.WorkoutType, AppDalDTO.WorkoutType>,
    IWorkoutTypeService
{
    public WorkoutTypeService(IAppUnitOfWork serviceUow, IWorkoutTypeRepository serviceRepository, IMapper mapper) 
        : base(serviceUow, serviceRepository, new WorkoutTypeMapper(mapper))
    {
    }
}