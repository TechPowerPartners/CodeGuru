using TestingPlatform.Api.Contracts.Dto;

namespace TestingPlatform.Api.Contracts.Requests;

public class CreateTestRequest
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public TimeOnly TravelTime { get; set; }
    public ICollection<CreateQuestionDto> Questions { get; set; } = default!;
}
