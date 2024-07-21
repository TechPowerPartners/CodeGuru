namespace TestingPlatform.Api.Contracts.Dto;

public class CreateAnswerDto
{
    public string Text { get; set; } = default!;
    public bool IsCorreсt { get; set; }
}