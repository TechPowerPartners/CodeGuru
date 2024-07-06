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
    Task<IApiResponse<TestDto>> GetTestAsync(GetTestRequest request);

    [Post("/tests/all")]
    Task<IApiResponse<IEnumerable<TestDto>>> GetAllTestsAsync();
}