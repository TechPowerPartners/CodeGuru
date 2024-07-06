using Refit;

namespace TG.Bot.Intagrations.BackendApi;

internal interface IBackendApi
{
    [Post("/tests/get")]
    Task<IApiResponse> GetTestAsync();
}