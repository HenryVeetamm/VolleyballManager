using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.BLL.DTO;

public class WorkoutType : DomainEntityId
{

    [MaxLength(64)] [MinLength(3)]
    [Column(TypeName = "jsonb")]
    public LangStr Description { get; set; } = new();

    public ICollection<Workout>? Workouts { get; set; }
}