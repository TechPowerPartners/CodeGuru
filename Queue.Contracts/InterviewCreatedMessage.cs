using Domain.Shared;

namespace Queue.Contracts;

public record InterviewCreatedMessage(string Name, CareerRole FromRole, CareerRole ToRole, DateTime FromTime, DateTime ToTime);