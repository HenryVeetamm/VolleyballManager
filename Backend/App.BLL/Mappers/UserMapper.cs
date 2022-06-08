using AutoMapper;
using Base.BLL.Mappers;
using AppDalDTO = App.DAL.DTO;
using AppBllDto = App.BLL.DTO;

namespace App.BLL.Mappers;

public class UserMapper : BaseMapper<AppBllDto.Identity.AppUser, AppDalDTO.Identity.AppUser>
{
    public UserMapper(IMapper mapper) : base(mapper)
    {
    }
}