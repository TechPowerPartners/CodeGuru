using Domain.Shared;

namespace Guard.Api.Contracts.Interviews;

/// <summary>
/// Èíôîðìàöèÿ äëÿ ñîçäàíèÿ ñîáåñåäîâàíèÿ.
/// </summary>
public class CreateInterviewRequest
{
    /// <summary>
    /// Âðåìÿ ïðîâåäåíèÿ - ñ.
    /// </summary>
    public DateTime FromTime { get; set; }

    /// <summary>
    /// Âðåìÿ ïðîâåäåíèÿ - ïî.
    /// </summary>
    public DateTime ToTime { get; set; }

    /// <summary>
    /// Ðîëü ñ êîòîðîé áóäåò ïîâûøåíèå.
    /// </summary>
    public CareerRole FromRole { get; set; }

    /// <summary>
    /// Ðîëü íà êîòîðóþ áóäåò ïîâûøåíèå.
    /// </summary>
    public CareerRole ToRole { get; set; }

    /// <summary>
    /// Èìÿ ñîáåñåäóåìîãî.
    /// </summary>
    public string IntervieweeName { get; set; }
}
