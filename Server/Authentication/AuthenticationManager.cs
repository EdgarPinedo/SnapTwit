using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Server.Authentication;
    
public class AuthenticationManager
{
    public (string, int) GenerateJwtToken(int id, string name, IConfiguration config)
    {
        var claimsIdentity = new ClaimsIdentity(new List<Claim>
        {
            new (ClaimTypes.Name, name),
            new (ClaimTypes.NameIdentifier, id.ToString())
        });

        var tokenExpiryTimeStamp = DateTime.UtcNow.AddDays(1);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256Signature);

        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = tokenExpiryTimeStamp,
            SigningCredentials = credentials
        };

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        var token = jwtSecurityTokenHandler.WriteToken(securityToken);
        int expiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds;

        return (token, expiresIn);
    }
}