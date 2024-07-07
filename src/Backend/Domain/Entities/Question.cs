namespace Domain.Entities;

/// <summary> Вопрос </summary>
public class Question
{
    /// <summary> Идентификатор вопроса </summary>
    public Guid Id { get; set; }

    /// <summary> Текст вопроса на который нужно ответить </summary>
    public string Text { get; set; } = default!;

    /// <summary> Количество баллов за правильный ответ на вопрос </summary>
    public int NumberOfPoints { get; set; }

    /// <summary> Уровень сложности вопроса </summary>
    public int DifficultyLevel { get; set; }

    /// <summary> Идентификатор теста </summary>
    public Guid TestId { get; set; }

    /// <summary> Файлы вопроса </summary>
    public ICollection<QuestionFiles>? Files { get; set; }

    /// <summary> Вырианты ответов </summary>
    public ICollection<Answer> Answers { get; set; } = default!;
}
