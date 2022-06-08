using AutoMapper;
using Base.DAL.EF.Mappers;
using AppDalDTO = App.DAL.DTO;
using AppDomain = App.Domain;

namespace DAL.App.EF.Mappers;

public class UserMapper : BaseMapper<AppDalDTO.Identity.AppUser, AppDomain.Identity.AppUser>
{
    public UserMapper(IMapper mapper) : base(mapper)
    {
    }
}