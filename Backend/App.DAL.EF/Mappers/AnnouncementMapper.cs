using AutoMapper;
using AppDalDto = App.DAL.DTO;
using AppDomain = App.Domain;
using Base.DAL.EF.Mappers;

namespace DAL.App.EF.Mappers;

public class AnnouncementMapper : BaseMapper<AppDalDto.Announcement, AppDomain.Announcement>
{
    public AnnouncementMapper(IMapper mapper) : base(mapper)
    {
    }
}