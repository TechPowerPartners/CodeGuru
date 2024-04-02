using Guard.Api.Domain;
using Guard.Api.DTOs.Users;
using Guard.Api.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Guard.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController(ApplicationDbContext _context) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        var findUser = _context.Users.FirstOrDefault(u => u.Name == dto.Name);

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ismailismailismailismailismailismailismail"));
        var loginCredential = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: "Swagger",
            audience: "Sms",
            claims: [],
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: loginCredential);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return Ok(tokenString);
    }

    [HttpPost("registration")]
    public async Task<IActionResult> RegistrationAsync(RegisterDto dto)
    {
        if (dto.Validator != 765123)
            return BadRequest("низя");

        var findUser = await _context.Users.FirstOrDefaultAsync(u => u.Name == dto.Name);

        if (findUser is not null)
        {
            findUser.Name = dto.Name;
            findUser.Password = dto.Password;
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status409Conflict, $"User already exist");
        }

        _context.Users.Add(new User
        {
            Name = dto.Name,
            Password = dto.Password,
        });

        await _context.SaveChangesAsync();

        return Ok();
    }
}
