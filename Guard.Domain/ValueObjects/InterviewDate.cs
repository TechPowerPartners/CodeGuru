namespace Guard.Domain.ValueObjects;

/// <summary>
/// Дата проведения собеседования.
/// </summary>
public readonly record struct InterviewDate
{
    /// <summary>
    /// С.
    /// </summary>
    public DateTime FromTime { get; init; }

    /// <summary>
    /// По.
    /// </summary>
    public DateTime ToTime { get; init; }

    public static InterviewDate Create(DateTime fromTime, DateTime toTime)
    {
        if (fromTime >= toTime)
            throw new ArgumentException("Невалидная дата собеседования");

        return new InterviewDate() {FromTime = fromTime, ToTime = toTime };
    }
}