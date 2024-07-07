using Api.Contracts.Tests.Dto;
using Api.Contracts.Tests.Requests;
using Api.Contracts.Users;
using Refit;

namespace TG.Bot.Intagrations.BackendApi;

internal interface IBackendApi
{
    [Post("/users/registration")]
    Task<IApiResponse> RegisterAsync(RegisterRequest request);

    [Post("/tests/get")]
    Task<IApiResponse<GetTestDto>> GetTestAsync(Guid id);

    [Post("/tests/gettestids")]
    Task<IApiResponse<ICollection<Guid>>> GetTestIdsAsync();
}