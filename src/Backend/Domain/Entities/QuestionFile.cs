namespace Domain.Entities;

/// <summary>
/// Промежуточная таблица для файла и вопроса
/// </summary>
public class QuestionFile
{
    /// <summary> Идентификатор </summary>
    public Guid Id { get; set; }

    /// <summary> Идентификатор вопроса </summary>
    public Guid QuestionId { get; set; }

    /// <summary> Идентификатор файла </summary>
    public Guid FileId { get; set; }
}
