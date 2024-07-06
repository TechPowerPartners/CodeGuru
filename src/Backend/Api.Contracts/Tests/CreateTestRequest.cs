using Domain.Entities;
namespace Api.Contracts.Tests;

public class CreateTestRequest
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public TimeOnly TravelTime { get; set; }
    public ICollection<Question> Questions { get; set; } = default!;
}