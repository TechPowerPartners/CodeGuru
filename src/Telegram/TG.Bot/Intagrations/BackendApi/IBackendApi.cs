using Api.Contracts.Tests.Dto;
using Api.Contracts.Tests.Requests;
using Api.Contracts.Users;
using Refit;

namespace TG.Bot.Intagrations.BackendApi;

internal interface IBackendApi
{
    [Post("/users/registration")]
    Task<IApiResponse> RegisterAsync(RegisterRequest request);

    [Get("/tests/{id}")]
    Task<IApiResponse<GetTestDto>> GetTestAsync(Guid id);

    [Get("/tests/NamesAndIds")]
    Task<IApiResponse<ICollection<GetTestNameAndIdDto>>> GetTestIdsAsync();
}