using Api.Contracts.Users;
using Api.Persistence;
using Api.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController(ApplicationDbContext _context, IPasswordHasher _passwordHasher) : ControllerBase
{
    TokenService tokenService = new TokenService();

    [HttpPost("login")]
	public IActionResult Login(LoginRequest request)
	{
		var findUser = _context.Users.FirstOrDefault(u => u.Name == request.Name);
        if (findUser == null)
        {
            return Unauthorized("Не существует такого пользователя");
        }
		var isPasswordValid = _passwordHasher.Verify(findUser.PasswordHash, request.Password);
        if (!isPasswordValid)
        {
            return Unauthorized("Не существует такого пользователя");
        }

		return Ok(tokenService.GenerateToken(request));
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
        UserDto userDto = new(
            userInfo.Name,
            userInfo.Id
            );
        return Ok(userDto);
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
			findUser.PasswordHash = request.Password;

			return StatusCode(StatusCodes.Status409Conflict, $"User already exist");
		}
        var hashedPassword = _passwordHasher.HashPassword(request.Password);
        _context.Users.Add(new User
		{
			Name = request.Name,
			PasswordHash = hashedPassword,
		});

        await _context.SaveChangesAsync();

		return Ok();
	}
    /// <summary>
    /// Добавь свойства 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name=""></param>
    public record UserDto(string userName,Guid userId);
}
