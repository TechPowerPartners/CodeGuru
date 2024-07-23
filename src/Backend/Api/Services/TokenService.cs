using Api.Contracts.Users;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services;

public class TokenService
{
    private static readonly byte[] _key = Encoding.ASCII.GetBytes("maflendmozarelladineshismailmishamaflendmozarelladineshismailmisha");

    public string GenerateToken(LoginRequest loginRequest, User user)
    {
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(_key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = CreateClaimsIdentity(),
            Expires = DateTime.UtcNow.AddMinutes(120),
            SigningCredentials = credentials
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);

        return handler.WriteToken(token);

        ClaimsIdentity CreateClaimsIdentity()
        {
            var claims = new ClaimsIdentity();

            claims.AddClaim(new(ClaimTypes.Name, loginRequest.Name));
            claims.AddClaim(new(ClaimTypes.NameIdentifier, user.Id.ToString()));

            return claims;
        }
    }
}
