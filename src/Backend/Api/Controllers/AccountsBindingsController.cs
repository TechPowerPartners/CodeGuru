using Api.Contracts.AccountBindings;
using Api.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/accounts-bindings")]
[ApiController]
public class AccountsBindingsController(ApplicationDbContext context) : ControllerBase
{
    [HttpPost("telegram")]
    public async Task<IActionResult> BindTelegramAccountAsync(BindTelegramAccountRequest request)
    {
        // TODO: Проверить, сущесвует ли пользователь а также совпадают пароли
        // В случае успеха в бд записать айди телеграм пользователя
        // Будет ли такое что можно перелинковать?()
        return Ok();
    }
}
