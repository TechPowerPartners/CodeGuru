using Api.Contracts.AccountBindings;
using Api.Persistence;
using Api.Services;
using Domain.Entities;
using EasyNetQ.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/accounts-bindings")]
[ApiController]
public class AccountsBindingsController(ApplicationDbContext _context, IPasswordHasher _passwordHasher) : ControllerBase
{
    [Authorize]
    [HttpPost("telegram")]
    public async Task<IActionResult> BindTelegramAccountAsync(BindTelegramAccountRequest request)
    {
        var user = _context.Users.FirstOrDefault(user => user.Name == request.Name);

        if (user == null)
            return Unauthorized("Неверный логин или пароль");

        var isCorrectPassword = _passwordHasher.Verify(user.PasswordHash, request.Password);

        if (!isCorrectPassword)
            return Unauthorized("Неверный логин или пароль");

        user.TelegramId = request.TelegramId;

        _context.Update(user);

        _context.SaveChanges();

        return Ok();
    }
}
