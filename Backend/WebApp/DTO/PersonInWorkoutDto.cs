using Base.Contracts.Domain;
using Base.Domain;

namespace WebApp.DTO;

public class PersonInWorkoutDto : DomainEntityId
{
    public string Comment { get; set; } = default!;
}