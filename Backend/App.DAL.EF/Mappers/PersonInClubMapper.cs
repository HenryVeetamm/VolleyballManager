using AutoMapper;
using Base.DAL.EF.Mappers;
using AppDalDto = App.DAL.DTO;
using AppDomain = App.Domain;

namespace DAL.App.EF.Mappers;

public class PersonInClubMapper: BaseMapper<AppDalDto.PersonInClub, AppDomain.PersonInClub>
{
    public PersonInClubMapper(IMapper mapper) : base(mapper)
    {
    }
}