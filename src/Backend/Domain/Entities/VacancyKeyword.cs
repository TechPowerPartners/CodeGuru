namespace Domain.Entities;

/// <summary>
/// Ключевое слово вакансии.
/// </summary>
public class VacancyKeyword
{
    public VacancyKeyword(string value) => Value = value;

    public string Value { get; set; }
}
