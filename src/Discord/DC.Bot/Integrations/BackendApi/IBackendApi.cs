using Api.Contracts.Interviews;
using Api.Contracts.Users;
using Refit;

namespace DC.Bot.Integrations.BackendApi;

public interface IBackendApi
{
	[Post("/users/registration")]
	Task<IApiResponse> RegisterAsync(RegisterRequest request);

	[Post("/interview")]
	Task<IApiResponse> CreateInterviewAsync(CreateInterviewRequest request);
}