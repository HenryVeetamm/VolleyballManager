using System.ComponentModel.DataAnnotations;

using Base.Contracts.Domain;
using Base.Domain;
using PublicAPI.DTO.v1.Identity;

namespace PublicAPI.DTO.v1;

public class PersonInWorkout
{
    public Guid Id { get; set; }
    
    public Guid WorkOutId { get; set; }
    public Workout? Workout { get; set; }
    
    public Guid AppUserId { get; set; }
    
    public AppUser? AppUser { get; set; }

    [MaxLength(128)]
    public string? Comment { get; set; }
}