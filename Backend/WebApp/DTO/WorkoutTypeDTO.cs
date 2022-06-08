using System.ComponentModel.DataAnnotations;
using App.Domain;
using Base.Domain;

namespace WebApp.DTO;

public class WorkoutTypeDTO : DomainEntityId
{
    [MaxLength(64)][MinLength(3)]
    public string Description { get; set; } = default!;
    
    public ICollection<Workout>? Workouts { get; set; }
}