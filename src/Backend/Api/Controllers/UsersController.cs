using Api.Contracts.Users;
using Api.Persistence;
using Api.Services;
using BCrypt.Net;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Api.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController(ApplicationDbContext _context) : ControllerBase
{
    AuthService auth = new AuthService();

    [HttpPost("login")]
	public IActionResult Login(LoginRequest request)
	{
		var findUser = _context.Users.FirstOrDefault(u => u.Name == request.Name);
        if (User == null)
        {
			return Unauthorized("Не существует такого пользователя");
        }
		var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, findUser.Password);
        if (!isPasswordValid)
        {
            return Unauthorized("Не существует такого пользователя");
        }

		return Ok(auth.GenerateToken(request));
	}
    [HttpGet("userinfo")]
    public async Task<IActionResult> GetUserInfo()
    {
        var userClaims = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        if (userClaims == null)
        {
            return Unauthorized("Не найдено имя пользователя в токене");
        }

        var userInfo = await _context.Users.FirstOrDefaultAsync(u => u.Name == userClaims);
        if (userInfo == null)
        {
            return NotFound("Пользователь не найден");
        }

        return Ok(userInfo);
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

			return StatusCode(StatusCodes.Status409Conflict, $"User already exist");
		}
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        _context.Users.Add(new User
		{
			Name = request.Name,
			Password = hashedPassword,
		});

		await _context.SaveChangesAsync();

		return Ok();
	}
    /// <summary>
    /// Добавь свойства 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name=""></param>
    public record UserDto(string userName,string userId);
}
