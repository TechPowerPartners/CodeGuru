using Api.Contracts.Users;
using Api.Persistence;
using Api.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController(ApplicationDbContext _context) : ControllerBase
{
    TokenService tokenService = new TokenService();

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

        return Ok(tokenService.GenerateToken(request, findUser));
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
}
