using Guard.Bot.GuardApiContracts.DTOs;
using Refit;

public interface IGuardApi
{
    [Post("/users/registration")]
    Task<IApiResponse> RegisterAsync(RegisterDto dto);

    [Get("/posts/random")]
    Task<IApiResponse<string>> GetRandomPostContentAsync();
}