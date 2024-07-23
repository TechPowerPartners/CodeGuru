using Api.Contracts.AccountBindings;
using Api.Persistence;
using Api.Services;
using Domain.Entities;
using EasyNetQ.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Route("api/accounts-bindings")]
[ApiController]
public class AccountsBindingsController(ApplicationDbContext _context, IPasswordHasher _passwordHasher) : ControllerBase
{
    [HttpPost("telegram")]
    public async Task<IActionResult> BindTelegramAccountAsync(BindTelegramAccountRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Name == request.Name);

        if (user == null)
            return Unauthorized("Неверный логин или пароль");

        var isCorrectPassword = _passwordHasher.Verify(user.PasswordHash, request.Password);

        if (!isCorrectPassword)
            return Unauthorized("Неверный логин или пароль");

        user.TelegramId = request.TelegramId;

        _context.Update(user);

        _context.SaveChanges();

        return Ok(user.Id);
    }

    [HttpGet("GetUserId/byTelegram/{id:long}")]
    public async Task<IActionResult> GetUserIdByTelegramIdAsync(long id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.TelegramId == id);

        if (user == null)
            return BadRequest("Not Found");

        return Ok(user.Id);
    }
}
