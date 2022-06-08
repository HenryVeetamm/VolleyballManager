using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class Club : DomainEntityId, IDomainAppUserId
{
    [MaxLength(64)] [MinLength(3)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.Club), Name = nameof(Name))]
    public string Name { get; set; } = null!;

    public ICollection<PersonInClub>? PersonInClubs { get; set; }

    public ICollection<Team>? Teams { get; set; }
    
    public bool OwnClub { get; set; }

    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}