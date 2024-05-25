using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// Собеседование.
/// </summary>
public class Interview
{
	private Interview() { }

	public Interview(Guid id, InterviewDate date, RoleEnhancement role, Guid intervieweeId)
	{
		Id = id;
		Date = date;
		Role = role;
		IntervieweeId = intervieweeId;
		State = InterviewState.Planned;
	}

	public Guid Id { get; set; }
	public InterviewDate Date { get; protected set; }
	public RoleEnhancement Role { get; protected set; }
	public DateTime? ComlitionDate { get; protected set; }
	public string? Comment { get; protected set; }
	public InterviewState State { get; protected set; }
	public InterviewResultStatus? ResultStatus { get; protected set; }
	public Guid IntervieweeId { get; protected set; }
	public User Interviewee { get; set; }

	/// <summary>
	/// Завершить.
	/// </summary>
	/// <param name="review">Отзыв.</param>
	/// <param name="status">Статус завершения.</param>
	/// <param name="complitionDate">Дата завершения.</param>
	public void Complete(string? review, InterviewResultStatus status, DateTime complitionDate)
	{
		Comment = review;
		ResultStatus = status;
		ComlitionDate = complitionDate;
		State = InterviewState.Completed;
	}

	/// <summary>
	/// Отменить.
	/// </summary>
	/// <param name="reason">Причина.</param>
	public void Cancel(string reason)
	{
		Comment = reason;
		State = InterviewState.Canceled;
	}
}
