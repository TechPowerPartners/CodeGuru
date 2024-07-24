using Api.Contracts.AccountBindings;
using Refit;

namespace TG.Bot.Intagrations.BackendApi;

internal interface IBackendApi
{
    [Post("/api/accounts-bindings/telegram")]
    Task<IApiResponse> BindTelegramAccountAsync(BindTelegramAccountRequest request);

    [Get("/api/accounts-bindings/telegram/{id}")]
    Task<IApiResponse<GetTelegramAccountBindingResponse>> GetTelegramAccountBindingAsync(long id);
}