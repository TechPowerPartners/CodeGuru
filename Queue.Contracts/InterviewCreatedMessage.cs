using Shared.Domain;

namespace Queue.Contracts;

public record InterviewCreatedMessage(string Name, CareerRole FromRole, CareerRole ToRole, DateOnly StartDate, TimeOnly FromTime, TimeOnly ToTime);