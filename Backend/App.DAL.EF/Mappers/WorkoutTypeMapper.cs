using AutoMapper;
using Base.DAL.EF.Mappers;
using AppDalDTO = App.DAL.DTO;
using AppDomain = App.Domain;
namespace DAL.App.EF.Mappers;

public class WorkoutTypeMapper : BaseMapper<AppDalDTO.WorkoutType, AppDomain.WorkoutType>
{
    public WorkoutTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}