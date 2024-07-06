namespace Domain.Entities;

/// <summary> Тест </summary>
public class Test
{
    /// <summary> Идентификатор вопроса </summary>
    public Guid Id { get; set; }

    /// <summary> Имя теста </summary>
    public string Name { get; set; } = default!;

    /// <summary> Описание теста </summary>
    public string? Description { get; set; }

    /// <summary> Время прохождения теста </summary>
    public TimeOnly TravelTime { get; set; }

    /// <summary> Вопросы </summary>
    public ICollection<Question> Questions { get; set; } = default!;
}
