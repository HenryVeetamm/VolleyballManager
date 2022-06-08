using AutoMapper;
using Base.BLL.Mappers;
using PublicAPI.DTO.v1.Identity;

namespace PublicAPI.DTO.v1.Mappers;

public class AnnouncementMapper : BaseMapper<Announcement, App.BLL.DTO.Announcement>
{
    public AnnouncementMapper(IMapper mapper) : base(mapper)
    {
    }

    public override Announcement Map(App.BLL.DTO.Announcement? inObject)
    {
        return new Announcement()
        {
            Id = inObject!.Id,
            AppUser = new AppUserSimple()
            {
                Id = inObject.AppUserId,
                FirstName = inObject!.AppUser!.FirstName,
                LastName = inObject!.AppUser!.LastName,
            },
            TeamId = inObject.TeamId!,
            Team = inObject!.Team != null ? new Team()
            {
                Name = inObject.Team!.Name,
                Code = inObject.Team!.Code,
                Club = inObject.Team.Club != null ? new Club()
                {
                    Name = inObject.Team.Club.Name
                } : null
            } : null,
            AppUserId = inObject!.AppUserId,
            Title = inObject.Title,
            Content = inObject.Content,
            Pinned = inObject.Pinned
        };
    }
    public  App.BLL.DTO.Announcement MapPost(AnnouncementPost? inObject)
    {
        return new App.BLL.DTO.Announcement()
        {
            Id = inObject!.Id,
            TeamId = inObject.TeamId!,
            Title = inObject.Title,
            Content = inObject.Content,
            Pinned = inObject.Pinned
        };
    }
}