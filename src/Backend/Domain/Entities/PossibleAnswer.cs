namespace Domain.Entities;

/// <summary>
/// Вариант ответа на вопрос
/// </summary>
public class PossibleAnswer
{
    /// <summary>
    /// Идентификатор ответа на вопрос
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Текст ответа
    /// </summary>
    public string Text { get; set; } = default!;

    /// <summary>
    /// Является ли ответ правильным на вопрос
    /// </summary>
    public bool IsCorreсt { get; set; }

    /// <summary>
    /// Идентификатор вопроса
    /// </summary>
    public Guid QuestionId { get; set; }
}