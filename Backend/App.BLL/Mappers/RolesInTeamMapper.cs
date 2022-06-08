using AutoMapper;
using Base.BLL.Mappers;
using AppDalDTO = App.DAL.DTO;
using AppBllDto = App.BLL.DTO;
namespace App.BLL.Mappers;

public class RolesInTeamMapper : BaseMapper<AppBllDto.RolesInTeam, AppDalDTO.RolesInTeam>
{
    public RolesInTeamMapper(IMapper mapper) : base(mapper)
    {
    }
}