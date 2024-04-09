using Guard.Api.DTOs.Interview;
using Guard.Bot.Integrations.GuardApi.DTOs;
using Refit;

namespace Guard.Bot.Integrations.GuardApi;

public interface IGuardApi
{
    [Post("/users/registration")]
    Task<IApiResponse> RegisterAsync(RegisterDto dto);

    [Get("/posts/random")]
    Task<IApiResponse<string>> GetRandomPostContentAsync();

    [Post("/interview")]
    Task<IApiResponse> CreateInterviewAsync(InterviewDto dto);
}