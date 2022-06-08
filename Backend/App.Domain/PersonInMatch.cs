using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;
[Index(nameof(MatchId), nameof(AppUserId), IsUnique = true)]
public class PersonInMatch : DomainEntityId, IDomainAppUserId
{
    public Guid AppUserId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.PersonInMatch), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }

    public Guid MatchId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.PersonInMatch), Name = nameof(Match))]
    public Match? Match { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.PersonInMatch), Name = nameof(TotalPoints))]
    public int? TotalPoints { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.PersonInMatch), Name = nameof(Aces))]
    public int? Aces { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.PersonInMatch), Name = nameof(Faults))]
    public int? Faults { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.PersonInMatch), Name = nameof(Reception))]
    public int? Reception { get; set; }
}