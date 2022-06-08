using AutoMapper;
using Base.DAL.EF.Mappers;
using AppDalDto = App.DAL.DTO;
using AppDomain = App.Domain;

namespace DAL.App.EF.Mappers;

public class ClubMapper : BaseMapper<AppDalDto.Club, AppDomain.Club>
{
    public ClubMapper(IMapper mapper) : base(mapper)
    {
    }
}