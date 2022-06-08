using AutoMapper;
using Base.DAL.EF.Mappers;
using AppDalDto = App.DAL.DTO;
using AppDomain = App.Domain;

namespace DAL.App.EF.Mappers;

public class MatchMapper : BaseMapper<AppDalDto.Match, AppDomain.Match>
{
    public MatchMapper(IMapper mapper) : base(mapper)
    {
    }
}