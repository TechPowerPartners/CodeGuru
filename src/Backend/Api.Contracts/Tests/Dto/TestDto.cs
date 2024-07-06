namespace Api.Contracts.Tests.Dto;

public class TestDto
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public TimeOnly TravelTime { get; set; }
    public ICollection<QuestionDto> Questions { get; set; } = default!;
}
