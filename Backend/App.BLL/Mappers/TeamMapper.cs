using AutoMapper;
using Base.BLL.Mappers;
using AppDalDTO = App.DAL.DTO;
using AppBllDto = App.BLL.DTO;
namespace App.BLL.Mappers;

public class TeamMapper : BaseMapper<AppBllDto.Team ,AppDalDTO.Team>
{
    public TeamMapper(IMapper mapper) : base(mapper)
    {
    }
}