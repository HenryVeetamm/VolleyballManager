using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;

[Index(nameof(ClubId), nameof(AppUserId), IsUnique = true)]
public class PersonInClub : DomainEntityId, IDomainAppUserId
{
    public Guid ClubId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.PersonInClub), Name = nameof(Club))]
    public Club? Club { get; set; }

    public Guid AppUserId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.PersonInClub), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
    
}