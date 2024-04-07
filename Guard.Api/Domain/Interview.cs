namespace Guard.Api.Domain;

public class Interview
{
    public Guid Id { get; set; }
    public DateOnly StartDate {  get; set; }
    public TimeOnly FromTime { get; set; }
    public TimeOnly ToTime { get; set; }

    public CareerRole FromRole { get; set; }
    public CareerRole ToRole { get; set; }
    public string Review { get; set; }
    public bool? IsPassed { get; set; } 
    public Guid IntervieweeId { get; set; }
    public User Interviewee { get; set; }

}




public enum CareerRole
{
    Amateur = 0,
    AmateurPlus = 1,
    Intern = 2,
    InterPlus = 3,
    Junior = 4,
    Middle = 5,
    Senior = 6,
}