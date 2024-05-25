namespace Domain.Entities;

/// <summary>Вакансия.</summary>
public class Vacancy
{
    public Vacancy(Guid id, string title, string description, Guid leaderId, ICollection<VacancyKeyword> keywords)
    {
        Id = id;
        Title = title;
        Description = description;
        LeaderId = leaderId;
        Keywords = keywords;
    }

    public Guid Id { get; set; }

    /// <summary>Короткое название.</summary>
    public string Title { get; set; }

    /// <summary>Полное описание.</summary>
    public string Description { get; set; }

    /// <summary>Идентификатор руководителя. </summary>
    public Guid LeaderId { get; set; }

    /// <summary>Руководитель.</summary>
    public User Leader { get; set; } = null!;

    /// <summary>Дата публикации.</summary>
    public DateTime PublicationDate { get; set; }

    /// <summary>Ключевые слова.</summary>
    public ICollection<VacancyKeyword> Keywords { get; set; }
    
    /// <summary>Кандидаты.</summary>
    public ICollection<Candidate> Candidates { get; set; }
}
