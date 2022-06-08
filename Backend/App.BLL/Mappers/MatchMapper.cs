using AutoMapper;
using Base.BLL.Mappers;
using AppDalDto = App.DAL.DTO;
using AppBllDto = App.BLL.DTO;

namespace App.BLL.Mappers;

public class MatchMapper : BaseMapper<AppBllDto.Match, AppDalDto.Match>
{
    public MatchMapper(IMapper mapper) : base(mapper)
    {
    }
}