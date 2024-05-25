using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Кандидат на вакансию.
/// </summary>
public class Candidate
{
    public Candidate(Guid vacancyId,Guid userId)
    {
        VacancyId = vacancyId;
        UserId = userId;
        State = CandidateState.InProcess;
    }

    public Guid VacancyId { get; set; }
    public Vacancy Vacancy { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public CandidateState State { get; set; }
}
