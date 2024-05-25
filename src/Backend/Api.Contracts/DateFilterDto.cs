namespace Api.Contracts;

/// <summary>
/// Фильтр по дате
/// </summary>
public record DateFilterDto
{
    public DateFilterDto(DateTime? from, DateTime? to)
    {
        From = from;
        To = to;
    }

    public DateFilterDto(DateOnly? target)
    {
        Target = target;
    }

    /// <summary>
    /// Если указана, будут получены данные у которых фильтруемая дата равна этой дате
    /// </summary>
    public DateOnly? Target { get; }

    /// <summary>
    /// Если указана, будут получены данные у которых фильтруемая дата начинается с этой даты
    /// </summary>
    public DateTime? From { get; }

    /// <summary>
    /// Если указана, будут получены данные у которых фильтруемая дата начинается до этой даты
    /// </summary>
    public DateTime? To { get; }
}
 