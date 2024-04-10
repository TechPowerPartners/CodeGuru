using Domain.Shared;

namespace Guard.Api.Contracts.Interviews;

/// <summary>
/// Информация для создания собеседования.
/// </summary>
public class CreateInterviewRequest
{
    /// <summary>
    /// Дата начала.
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Время проведения - с.
    /// </summary>
    public TimeOnly FromTime { get; set; }

    /// <summary>
    /// Время проведения - по.
    /// </summary>
    public TimeOnly ToTime { get; set; }

    /// <summary>
    /// Роль с которой будет повышение.
    /// </summary>
    public CareerRole FromRole { get; set; }

    /// <summary>
    /// Роль на которую будет повышение.
    /// </summary>
    public CareerRole ToRole { get; set; }

    /// <summary>
    /// Имя собеседуемого.
    /// </summary>
    public string IntervieweeName { get; set; }
}
