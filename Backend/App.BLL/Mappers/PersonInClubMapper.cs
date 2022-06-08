using AutoMapper;
using Base.BLL.Mappers;
using AppDalDto = App.DAL.DTO;
using AppBllDto = App.BLL.DTO;

namespace App.BLL.Mappers;

public class PersonInClubMapper : BaseMapper<AppBllDto.PersonInClub,AppDalDto.PersonInClub>
{
    public PersonInClubMapper(IMapper mapper) : base(mapper)
    {
    }
}