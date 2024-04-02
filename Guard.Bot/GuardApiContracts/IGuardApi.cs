using Guard.Bot.GuardApiContracts.DTOs;
using Refit;

public interface IGuardApi
{
    [Post("/users/registration")]
    Task RegisterAsync(RegisterDto dto);

    [Get("/posts/random")]
    Task<string> GetRandomPostContentAsync();
}