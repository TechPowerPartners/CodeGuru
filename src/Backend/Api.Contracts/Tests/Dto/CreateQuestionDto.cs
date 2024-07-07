namespace Api.Contracts.Tests.Dto;

public class CreateQuestionDto
{
    public string Text { get; set; } = default!;
    public int NumberOfPoints { get; set; }
    public int DifficultyLevel { get; set; }

    //TODO: Добработать файлы
    //public ICollection<Guid>? FileIds { get; set; }
    public ICollection<CreateAnswerDto> Answers { get; set; } = default!;
}
