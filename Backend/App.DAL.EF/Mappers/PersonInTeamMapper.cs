using AutoMapper;
using Base.DAL.EF.Mappers;
using AppDalDto = App.DAL.DTO;
using AppDomain = App.Domain;
namespace DAL.App.EF.Mappers;

public class PersonInTeamMapper : BaseMapper<AppDalDto.PersonInTeam, AppDomain.PersonInTeam>
{
    public PersonInTeamMapper(IMapper mapper) : base(mapper)
    {
    }
}