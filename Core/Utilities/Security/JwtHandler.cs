/*using Core.Concrete.Constants;
using Core.Concrete.Constants.Enum;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Abstract.Interface;

namespace Core.Utilities.Security;

public static class JwtHandler
{
    public static string GenerateToken(IUser user, Application application)
    {
        string secretKey;
        string issuer;
        string audience;

        switch (application)
        {
            case Application.Admin:
                secretKey = ApplicationJwtSettings.AdminSecretKey;
                issuer = ApplicationJwtSettings.AdminIssuer;
                audience = ApplicationJwtSettings.AdminAudience;
                break;
            case Application.Store:
                secretKey = ApplicationJwtSettings.StoreSecretKey;
                issuer = ApplicationJwtSettings.StoreIssuer;
                audience = ApplicationJwtSettings.StoreAudience;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(application), application, @"Invalid application type.");
        }

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

        var dateTimeNow = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: new List<Claim>
            {
                new("email", user.EmailAddress)
            },
            notBefore: dateTimeNow,
            expires: dateTimeNow.Add(TimeSpan.FromMinutes(500)),
            signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}*/