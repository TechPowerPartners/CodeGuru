using Api.Contracts.Users;
using Api.Persistence;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Api.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController(ApplicationDbContext _context) : ControllerBase
{
	[HttpPost("login")]
	public IActionResult Login(LoginRequest request)
	{
		var findUser = _context.Users.FirstOrDefault(u => u.Name == request.Name);
		if (findUser != null)
		{
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
		return BadRequest("Не существует такого пользователя");
	}

	[HttpPost("registration")]
	public async Task<IActionResult> RegistrationAsync(RegisterRequest request)
	{
		if (request.Validator != 765123)
			return BadRequest("низя");

		var findUser = await _context.Users.FirstOrDefaultAsync(u => u.Name == request.Name);

		if (findUser is not null)
		{
			findUser.Name = request.Name;
			findUser.Password = request.Password;
			await _context.SaveChangesAsync();

			return StatusCode(StatusCodes.Status409Conflict, $"User already exist");
		}

		_context.Users.Add(new User
		{
			Name = request.Name,
			Password = request.Password,
		});

		await _context.SaveChangesAsync();

		return Ok();
	}
}
