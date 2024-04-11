using Domain.Shared;

namespace Guard.Api.Contracts.Interviews;

/// <summary>
/// Информация для создания собеседования.
/// </summary>
public class CreateInterviewRequest
{
	/// <summary>
	/// С.
	/// </summary>
	public DateTime FromDate { get; set; }

	/// <summary>
	/// По.
	/// </summary>
	public DateTime ToDate { get; set; }

	/// <summary>
	/// С роли.
	/// </summary>
	public CareerRole FromRole { get; set; }

	/// <summary>
	/// На роль.
	/// </summary>
	public CareerRole ToRole { get; set; }

	/// <summary>
	/// Имя собеседуемого.
	/// </summary>
	public string IntervieweeName { get; set; } = string.Empty;
}
