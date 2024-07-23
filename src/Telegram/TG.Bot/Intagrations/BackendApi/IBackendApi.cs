using Api.Contracts.AccountBindings;
using Api.Contracts.Users;
using Refit;

namespace TG.Bot.Intagrations.BackendApi;

internal interface IBackendApi
{
    [Post("/users/login")]
    Task<IApiResponse<string>> LoginAsync(LoginRequest request);

    [Post("/api/accounts-bindings/telegram")]
    Task<IApiResponse> BindTelegramAccountAsync(BindTelegramAccountRequest request);

    [Get("/api/accounts-bindings/telegram/{id}")]
    Task<IApiResponse<GetTelegramAccountBindingResponse>> GetTelegramAccountBindingAsync(long id);
}