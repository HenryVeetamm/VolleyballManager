using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.Identity;
using Base.Contracts.Domain;
using Base.Domain;


namespace App.BLL.DTO;

public class Workout : DomainEntityId, IDomainAppUserId
{
    public Guid WorkoutTypeId { get; set; }
    public WorkoutType? WorkoutType { get; set; }

    [MaxLength(512)]
    public string? Description { get; set; }
    
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime Date { get; set; }

    public ICollection<PersonInWorkout>? PersonInWorkouts { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}