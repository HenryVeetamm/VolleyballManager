using AutoMapper;
using Base.BLL.Mappers;
using AppDalDTO = App.DAL.DTO;
using AppBllDto = App.BLL.DTO;
namespace App.BLL.Mappers;

public class WorkoutTypeMapper : BaseMapper<AppBllDto.WorkoutType ,AppDalDTO.WorkoutType>
{
    public WorkoutTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}