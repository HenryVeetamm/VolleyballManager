using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.Identity;
using Base.Contracts.Domain;
using Base.Domain;


namespace App.BLL.DTO;

public class PersonInWorkout : DomainEntityId, IDomainAppUserId
{
    public Guid WorkOutId { get; set; }
    public Workout? Workout { get; set; }
    
    public Guid AppUserId { get; set; }
    
    public AppUser? AppUser { get; set; }

    [MaxLength(128)]
    public string? Comment { get; set; }
}