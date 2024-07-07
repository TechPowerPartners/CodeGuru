namespace Domain.Entities;

/// <summary> Файл для вопроса </summary>
public class QuestionFiles
{
    /// <summary> Идентификатор файла </summary>
    public Guid Id { get; set; }

    /// <summary> Путь файла </summary>
    public string Path { get; set; } = default!;
}
