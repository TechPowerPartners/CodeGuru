namespace Api.Contracts.Tests.Dto;

public class GetTestDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public TimeOnly TravelTime { get; set; }
    public ICollection<GetQuestionDto> Questions { get; set; } = default!;
}
