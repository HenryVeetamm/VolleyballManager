using AutoMapper;
using Base.BLL.Mappers;
using AppDalDTO = App.DAL.DTO;
using AppBllDto = App.BLL.DTO;
namespace App.BLL.Mappers;

public class WorkoutMapper : BaseMapper<AppBllDto.Workout, AppDalDTO.Workout>
{
    public WorkoutMapper(IMapper mapper) : base(mapper)
    {
    }
    
}