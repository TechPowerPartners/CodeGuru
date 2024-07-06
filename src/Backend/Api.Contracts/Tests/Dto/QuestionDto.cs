namespace Api.Contracts.Tests.Dto;

public class QuestionDto
{
    public string Text { get; set; } = default!;
    public int NumberOfPoints { get; set; }
    public int DifficultyLevel { get; set; }
    public ICollection<Guid>? FileIds { get; set; }
    public ICollection<AnswerDto> Answers { get; set; } = default!;
}
