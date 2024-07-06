namespace Api.Contracts.Tests.Dto;

public class GetQuestionDto
{
    public Guid Id { get; set; }
    public string Text { get; set; } = default!;
    public int NumberOfPoints { get; set; }
    public int DifficultyLevel { get; set; }
    public ICollection<GetAnswerDto> Answers { get; set; } = default!;
}
