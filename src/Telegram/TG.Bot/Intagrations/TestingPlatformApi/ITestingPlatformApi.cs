using Refit;
using TestingPlatform.Api.Contracts.Dto;

namespace TG.Bot.Intagrations.TestingPlatformApi;

internal interface ITestingPlatformApi
{
    [Get("/api/tests/{id}")]
    Task<IApiResponse<GetTestDto>> GetTestAsync(Guid id);

    [Get("/api/tests/NamesAndIds")]
    Task<IApiResponse<ICollection<GetTestNameAndIdDto>>> GetTestNamesAndIdsAsync();
}
