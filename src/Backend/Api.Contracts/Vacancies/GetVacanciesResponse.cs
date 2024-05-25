namespace Api.Contracts.Vacancies;
public class GetVacanciesResponse
{
    public ICollection<GetVacancyDto> Vacancies { get; set; }
}

public record GetVacancyDto
{

}
