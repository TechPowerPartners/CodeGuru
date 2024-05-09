namespace Domain.Enums;

/// <summary>
/// Состояние собеседования.
/// </summary>
public enum InterviewState
{
	/// <summary>
	/// Запланировано.
	/// </summary>
	Planned = 0,

	/// <summary>
	/// Завершено.
	/// </summary>
	Completed = 1,

	/// <summary>
	/// Отменено.
	/// </summary>
	Canceled = 2,
}
