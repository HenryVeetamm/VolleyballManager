using AutoMapper;
using Base.DAL.EF.Mappers;
using AppDalDto = App.DAL.DTO;
using AppDomain = App.Domain;
namespace DAL.App.EF.Mappers;

public class PersonInWorkoutMapper : BaseMapper<AppDalDto.PersonInWorkout, AppDomain.PersonInWorkout>
{
    public PersonInWorkoutMapper(IMapper mapper) : base(mapper)
    {
    }
}