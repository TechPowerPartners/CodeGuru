namespace Api.Contracts.Tests;

public class QuestionDto
{
    public string Text { get; set; } = default!;
    public int NumberOfPoints { get; set; }
    public int DifficultyLevel { get; set; }
    public ICollection<Guid>? FileIds { get; set; }
    public ICollection<AnswerDto> PossibleAnswers { get; set; } = default!;
}
