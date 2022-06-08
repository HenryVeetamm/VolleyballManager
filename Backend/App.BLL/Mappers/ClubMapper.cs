using AutoMapper;
using Base.BLL.Mappers;
using AppDalDto = App.DAL.DTO;
using AppBllDto = App.BLL.DTO;

namespace App.BLL.Mappers;

public class ClubMapper : BaseMapper<AppBllDto.Club, AppDalDto.Club>
{
    public ClubMapper(IMapper mapper) : base(mapper)
    {
    }
}