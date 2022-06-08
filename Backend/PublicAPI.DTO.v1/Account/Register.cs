using PublicAPI.DTO.v1.Enums;

namespace PublicAPI.DTO.v1.Account;

public class Register
{
    
    public string Email { get; set; } = default!;
    
    public string FirstName { get; set; } = default!;
    
    public string LastName { get; set; } = default!;
    
    public DateTime Birthday { get; set; } = default!;

    public EGender Gender { get; set; } = default!;

    public string Role { get; set; } = default!;

    public string Nationalcode { get; set; } = default!;

    public string Password { get; set; } = default!;
}