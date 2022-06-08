using AutoMapper;
using Base.DAL.EF.Mappers;
using AppDalDTO = App.DAL.DTO;
using AppDomain = App.Domain;
namespace DAL.App.EF.Mappers;

public class TeamMapper : BaseMapper<AppDalDTO.Team, AppDomain.Team>
{
    public TeamMapper(IMapper mapper) : base(mapper)
    {
    }
}