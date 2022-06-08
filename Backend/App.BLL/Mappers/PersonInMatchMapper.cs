using AutoMapper;
using Base.BLL.Mappers;
using AppDalDto = App.DAL.DTO;
using AppBllDto = App.BLL.DTO;

namespace App.BLL.Mappers;

public class PersonInMatchMapper : BaseMapper<AppBllDto.PersonInMatch, AppDalDto.PersonInMatch>
{
    public PersonInMatchMapper(IMapper mapper) : base(mapper)
    {
    }
}