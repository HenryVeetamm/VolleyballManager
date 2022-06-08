using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class Workout : DomainEntityId, IDomainAppUserId
{
    public Guid WorkoutTypeId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Workout), Name = nameof(WorkoutType))]
    public WorkoutType? WorkoutType { get; set; }

    [MaxLength(512)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.Workout), Name = nameof(Description))]
    public string? Description { get; set; }
    
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [Display(ResourceType = typeof(App.Resources.App.Domain.Workout), Name = nameof(Date))]
    public DateTime Date { get; set; }

    public ICollection<PersonInWorkout>? PersonInWorkouts { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}