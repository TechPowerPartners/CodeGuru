using Shared.Domain;

namespace Guard.Api.DTOs.Interview;

public class InterviewDto
{
    public DateOnly StartDate { get; set; }
    public TimeOnly FromTime { get; set; }
    public TimeOnly ToTime { get; set; }
    public CareerRole FromRole { get; set; }
    public CareerRole ToRole { get; set; }
    public string Name { get; set; }
}
