using Guard.Api.Contracts.Interviews;
using Guard.Api.Contracts.Users;
using Refit;

namespace Guard.Bot.Integrations.GuardApi;

public interface IGuardApi
{
    [Post("/users/registration")]
    Task<IApiResponse> RegisterAsync(RegisterRequest request);

    [Get("/posts/random")]
    Task<IApiResponse<string>> GetRandomPostContentAsync();

    [Post("/interview")]
    Task<IApiResponse> CreateInterviewAsync(CreateInterviewRequest request);
}