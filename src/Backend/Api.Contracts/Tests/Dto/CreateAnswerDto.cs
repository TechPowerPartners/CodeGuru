namespace Api.Contracts.Tests.Dto;

public class CreateAnswerDto
{
    public string Text { get; set; } = default!;
    public bool IsCorreсt { get; set; }
}