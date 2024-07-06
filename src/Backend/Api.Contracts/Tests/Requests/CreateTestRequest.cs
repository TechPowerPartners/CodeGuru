using Api.Contracts.Tests.Dto;

namespace Api.Contracts.Tests.Requests;

public class CreateTestRequest
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public TimeOnly TravelTime { get; set; }
    public ICollection<QuestionDto> Questions { get; set; } = default!;
}
