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

        await _context.SaveChangesAsync();

        return Ok(user.Id);
    }

    [HttpGet("telegram/{telegramId:long}")]
    public async Task<IActionResult> GetTelegramAccountBindingAsync(long telegramId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.TelegramId == telegramId);

        if (user == null)
            return NotFound("Not Found");

        var response = new GetTelegramAccountBindingResponse()
        {
            UserId = user.Id,
            TelegramId = user.TelegramId!.Value,
        };

        return Ok(response);
    }
}
