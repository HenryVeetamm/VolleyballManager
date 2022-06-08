using System.ComponentModel.DataAnnotations;
using App.DAL.DTO.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.DAL.DTO;

public class Club : DomainEntityId, IDomainAppUserId
{
    [MaxLength(64)] [MinLength(3)]
    public string Name { get; set; } = null!;

    public ICollection<PersonInClub>? PersonInClubs { get; set; }

    public ICollection<Team>? Teams { get; set; }
    
    public bool OwnClub { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}