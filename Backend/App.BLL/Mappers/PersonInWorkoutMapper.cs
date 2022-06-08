using AutoMapper;
using Base.BLL.Mappers;
using AppDalDto = App.DAL.DTO;
using AppBllDto = App.BLL.DTO;
namespace App.BLL.Mappers;

public class PersonInWorkoutMapper : BaseMapper<AppBllDto.PersonInWorkout, AppDalDto.PersonInWorkout>
{
    public PersonInWorkoutMapper(IMapper mapper) : base(mapper)
    {
    }
}