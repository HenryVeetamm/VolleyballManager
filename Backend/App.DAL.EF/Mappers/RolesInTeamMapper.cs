using AutoMapper;
using Base.DAL.EF.Mappers;
using AppDalDTO = App.DAL.DTO;
using AppDomain = App.Domain;
namespace DAL.App.EF.Mappers;

public class RolesInTeamMapper : BaseMapper<AppDalDTO.RolesInTeam, AppDomain.RolesInTeam>
{
    public RolesInTeamMapper(IMapper mapper) : base(mapper)
    {
    }
}