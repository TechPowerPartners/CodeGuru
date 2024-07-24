namespace TestingPlatform.Domain.Entities;

/// <summary> Файл для вопроса </summary>
public class QuestionFile
{
    /// <summary> Идентификатор файла </summary>
    public Guid Id { get; set; }

    /// <summary> Путь файла </summary>
    public string Path { get; set; } = default!;
}
