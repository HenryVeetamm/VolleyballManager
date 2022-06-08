using AutoMapper;
using Base.BLL.Mappers;
using AppBllDTO = App.BLL.DTO;
using AppDalDTO = App.DAL.DTO;

namespace App.BLL.Mappers;

public class AnnouncementMapper : BaseMapper<AppBllDTO.Announcement,AppDalDTO.Announcement>
{
    public AnnouncementMapper(IMapper mapper) : base(mapper)
    {
    }
}