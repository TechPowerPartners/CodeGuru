namespace Guard.Domain.ValueObjects;

/// <summary>
/// Дата проведения собеседования.
/// </summary>
public readonly record struct InterviewDate
{
    /// <summary>
    /// Дата.
    /// </summary>
    public DateOnly Date { get; init; }

    /// <summary>
    /// С.
    /// </summary>
    public TimeOnly FromTime { get; init; }

    /// <summary>
    /// По.
    /// </summary>
    public TimeOnly ToTime { get; init; }

    public static InterviewDate Create(DateOnly date, TimeOnly fromTime, TimeOnly toTime)
    {
        if (fromTime >= toTime)
            throw new ArgumentException("Невалидная дата собеседования");

        return new InterviewDate() { Date = date, FromTime = fromTime, ToTime = toTime };
    }
}