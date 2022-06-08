using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace PublicAPI.DTO.v1;

public class Workout
{
    public Guid Id { get; set; }
    public Guid WorkoutTypeId { get; set; }
    public WorkoutType? WorkoutType { get; set; }

    [MaxLength(512)]
    public string? Description { get; set; }
    
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime Date { get; set; }
    
}