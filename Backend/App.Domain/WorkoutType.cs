using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class WorkoutType : DomainEntityId
{

    [MaxLength(64)] [MinLength(3)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.WorkoutType), Name = nameof(Description))]
    [Column(TypeName = "jsonb")] 
    public LangStr Description { get; set; } = new();

    public ICollection<Workout>? Workouts { get; set; }
}