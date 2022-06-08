using System.ComponentModel.DataAnnotations;

using Base.Domain;


namespace PublicAPI.DTO.v1.Identity;

public class AppUser
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime Birthday { get; set; }
    
    public string NationalCode { get; set; } = null!;
}

public class AppUserSimple : DomainEntityId
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}