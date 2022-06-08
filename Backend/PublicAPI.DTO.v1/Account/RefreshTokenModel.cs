namespace PublicAPI.DTO.v1.Account;

public class RefreshTokenModel
{
    public string Jwt { get; set; } = default!;
    
    public string RefreshToken { get; set; } = default!;
}