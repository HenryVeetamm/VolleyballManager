using AutoMapper;
using Base.DAL.EF.Mappers;
using AppDalDto = App.DAL.DTO;
using AppDomain = App.Domain;

namespace DAL.App.EF.Mappers;

public class PersonInMatchMapper : BaseMapper<AppDalDto.PersonInMatch, AppDomain.PersonInMatch>
{
    public PersonInMatchMapper(IMapper mapper) : base(mapper)
    {
    }
}