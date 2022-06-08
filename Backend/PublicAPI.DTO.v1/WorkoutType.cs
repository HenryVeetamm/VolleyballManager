using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace PublicAPI.DTO.v1;

public class WorkoutType
{
    public Guid Id { get; set; }

    [MaxLength(64)] [MinLength(3)]
    public string Description { get; set; } = default!;
    
}