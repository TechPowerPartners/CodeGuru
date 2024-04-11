namespace Guard.Domain.ValueObjects;

/// <summary>
/// Дата проведения собеседования.
/// </summary>
public readonly record struct InterviewDate
{
	private InterviewDate(DateTime from, DateTime to)
	{
		From = from;
		To = to;
	}

	/// <summary>
	/// С.
	/// </summary>
	public DateTime From { get; init; }

	/// <summary>
	/// По.
	/// </summary>
	public DateTime To { get; init; }

	public static InterviewDate Create(DateTime from, DateTime to)
	{
		return from >= to ? throw new ArgumentException("Невалидная дата собеседования") : new InterviewDate(from, to);
	}
}