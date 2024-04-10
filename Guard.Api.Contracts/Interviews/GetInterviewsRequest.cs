namespace Guard.Api.Contracts.Interviews;

/// <summary>
/// Информация для получения собеседований.
/// </summary>
public class GetInterviewsRequest
{
    /// <summary>
    /// Дата начала собеседования.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Имя собеседуемого.
    /// </summary>
    public string? IntervieweeName { get; set; }
}