using Refit;
using TestingPlatform.Api.Contracts.Dto;

namespace TG.Bot.CacheServices.Base;

internal interface ICachedTestingPlatformApiService
{
    Task<IApiResponse<ICollection<GetTestNameAndIdDto>>> GetTestNamesAndIdsAsync();
}
