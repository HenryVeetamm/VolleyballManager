using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class Announcement : DomainEntityId, IDomainAppUserId
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    public Guid? TeamId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Announcement), Name = nameof(Team))]
    public Team? Team { get; set; }

    [MaxLength(64)] [MinLength(3)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.Announcement), Name = nameof(Title))]
    public string Title { get; set; } = null!;

    [MaxLength(512)] [MinLength(3)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.Announcement), Name = nameof(Content))]
    public string Content { get; set; } = null!;
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Announcement), Name = nameof(Pinned))]
    public bool Pinned { get; set; }
}