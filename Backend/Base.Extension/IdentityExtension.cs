﻿using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Base.Extension;

public static class IdentityExtension
{
    public static Guid GetUserId(this ClaimsPrincipal user) => GetUserId<Guid>(user);

    public static TKeyType GetUserId<TKeyType>(this ClaimsPrincipal user)
    {
        /*if (typeof(TKeyType) != typeof(Guid) || 
            typeof(TKeyType) != typeof(string) || 
            typeof(TKeyType) != typeof(int))
        {
            throw new ApplicationException($"This type of User id {typeof(TKeyType).Name} is not supported!");
        }*/

        var idClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (idClaim == null)
        {
            throw new NullReferenceException("NameIdentifier claim not found");
        }

        var res = (TKeyType)TypeDescriptor.GetConverter(typeof(TKeyType)).ConvertFromInvariantString(idClaim.Value)!;

        return res;
    }

    public static string GenerateJwt(IEnumerable<Claim> claims,
        string key,
        string issuer, string audience,
        DateTime expirationDateTime)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer, 
            audience,
            claims, 
            expires: expirationDateTime,
            signingCredentials: signingCredentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static bool ValidateJwtSignature(string jwtToken, string key, string validIssuer, string validAudience)
    {
        // validate jwt token signature.
        var tokenHandler = new JwtSecurityTokenHandler();
       
        try
        {
            tokenHandler.ValidateToken(jwtToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateLifetime = false,
                ValidIssuer = validIssuer,
                ValidAudience = validAudience
               
               
            }, out SecurityToken validatedToken);
          
            
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}