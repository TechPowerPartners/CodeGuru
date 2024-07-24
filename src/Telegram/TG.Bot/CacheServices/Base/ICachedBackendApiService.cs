using Api.Contracts.AccountBindings;
using Refit;

namespace TG.Bot.CacheServices.Base;

internal interface ICachedBackendApiService
{
    Task<IApiResponse<GetTelegramAccountBindingResponse>> GetTelegramAccountBindingAsync(long id);
}