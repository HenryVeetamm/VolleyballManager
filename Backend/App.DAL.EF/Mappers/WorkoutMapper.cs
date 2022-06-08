using AutoMapper;
using Base.DAL.EF.Mappers;
using AppDalDTO = App.DAL.DTO;
using AppDomain = App.Domain;
namespace DAL.App.EF.Mappers;

public class WorkoutMapper : BaseMapper<AppDalDTO.Workout, AppDomain.Workout>
{
    public WorkoutMapper(IMapper mapper) : base(mapper)
    {
    }
}