using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;
using Base.Domain;
using PublicAPI.DTO.v1.Identity;


namespace PublicAPI.DTO.v1;

public class Announcement
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public AppUserSimple? AppUser { get; set; }

    public Guid? TeamId { get; set; }
    public Team? Team { get; set; }

    [MaxLength(64)] [MinLength(3)]
    public string Title { get; set; } = null!;

    [MaxLength(512)] [MinLength(3)]
    public string Content { get; set; } = null!;
    
    public bool Pinned { get; set; }
}

public class AnnouncementPost : DomainEntityId
{
    public Guid? TeamId { get; set; }

    [MaxLength(64)] [MinLength(3)]
    public string Title { get; set; } = null!;

    [MaxLength(512)] [MinLength(3)]
    public string Content { get; set; } = null!;
    
    public bool Pinned { get; set; }
}