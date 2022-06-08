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

public class PersonInWorkoutService: BaseEntityService<IAppUnitOfWork, IPersonInWorkoutRepository, AppBllDTO.PersonInWorkout, AppDalDTO.PersonInWorkout> 
    , IPersonInWorkoutService
{
    public PersonInWorkoutService(IAppUnitOfWork serviceUow, IPersonInWorkoutRepository serviceRepository, IMapper mapper) 
        : base(serviceUow, serviceRepository, new PersonInWorkoutMapper(mapper))
    {
    }

    public async Task<IEnumerable<AppBllDTO.PersonInWorkout>> GetAllPersonInWorkoutByWorkoutId(Guid workoutId, bool noTracking = true)
    {
        return (await ServiceUow.PersonInWorkout.GetAllPersonInWorkoutByWorkoutId(workoutId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}