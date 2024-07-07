using Api.Contracts.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services
{
    public class AuthService
    {
        public string GenerateToken(LoginRequest loginRequest)
        {
            var handler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes("maflendmozarelladineshismailmishamaflendmozarelladineshismailmisha");

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
                {
                Subject = GenerateClaims(loginRequest),
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = credentials
                };

            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }


        private static ClaimsIdentity GenerateClaims(LoginRequest request)
        {
            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim(ClaimTypes.Name, request.Name));


            return claims;
        }

    }
}
