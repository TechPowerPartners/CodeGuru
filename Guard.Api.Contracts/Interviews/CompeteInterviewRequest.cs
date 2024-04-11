using Guard.Domain.Enums;

namespace Guard.Api.Contracts.Interviews;

/// <summary>
/// Информация для завершения собеседования.
/// </summary>
public class CompeteInterviewRequest
{
	/// <summary>
	/// Отзыв.
	/// </summary>
	public string? Review { get; set; }

	/// <summary>
	/// Статус.
	/// </summary>
	public InterviewResultStatus Status { get; set; }
}
