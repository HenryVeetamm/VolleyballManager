using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.BLL.DTO;

public class Announcement : DomainEntityId, IDomainAppUserId
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    public Guid? TeamId { get; set; }
    public Team? Team { get; set; }

    [MaxLength(64)] [MinLength(3)]
    public string Title { get; set; } = null!;

    [MaxLength(512)] [MinLength(3)]
    public string Content { get; set; } = null!;
    
    public bool Pinned { get; set; }
}