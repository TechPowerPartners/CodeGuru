namespace TestingPlatform.Api.Contracts.Dto;

public class GetTestNameAndIdDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
