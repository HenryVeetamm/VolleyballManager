using AutoMapper;
using Base.BLL.Mappers;
using AppDalDto = App.DAL.DTO;
using AppBllDto = App.BLL.DTO;
namespace App.BLL.Mappers;

public class PersonInTeamMapper : BaseMapper<AppBllDto.PersonInTeam, AppDalDto.PersonInTeam>
{
    public PersonInTeamMapper(IMapper mapper) : base(mapper)
    {
    }
}