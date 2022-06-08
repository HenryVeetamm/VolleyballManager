using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;

[Index(nameof(WorkOutId), nameof(AppUserId), IsUnique = true)]
public class PersonInWorkout : DomainEntityId, IDomainAppUserId
{
    public Guid WorkOutId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.PersonInWorkout), Name = nameof(Workout))]
    public Workout? Workout { get; set; }
    
    public Guid AppUserId { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.PersonInWorkout), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }

    [MaxLength(128)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.PersonInWorkout), Name = nameof(Comment))]
    public string? Comment { get; set; }
}