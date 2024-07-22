using Api.Contracts.AccountBindings;
using Api.Persistence;
using EasyNetQ.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/accounts-bindings")]
[ApiController]
public class AccountsBindingsController(ApplicationDbContext _context) : ControllerBase
{
    [Authorize]
    [HttpPost("telegram")]
    public async Task<IActionResult> BindTelegramAccountAsync(BindTelegramAccountRequest request)
    {
        var user = _context.Users.FirstOrDefault(user => user.Name == request.Name);

        if (user == null)
            return Unauthorized("Неверный логин или пароль");

        var isCorrectPassword = new Services.PasswordHasher().Verify(user.Password, request.Password);

        if (!isCorrectPassword)
            return Unauthorized("Неверный логин или пароль");

        // TODO: Сохранить в бд User

        return Ok();
    }
}
