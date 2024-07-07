namespace Api.Contracts.Tests.Dto;

public class GetAnswerDto
{
    public Guid Id { get; set; }
    public string Text { get; set; } = default!;
}