namespace Api.Contracts.Vacancies;
public record GetVacanciesRequest
{
    public IReadOnlyCollection<string>? Keywords { get; set; }
    public Guid? LeaderId { get; set; }
    public DateFilterDto? PublicationDate { get; set; }
    public PageRequest Page { get; set; }
}
