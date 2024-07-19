namespace Domain.Entities;

/// <summary>
/// Статья.
/// </summary>
public class Article
{
    /// <summary>
    /// Идентификатор статьи.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Содержимое.
    /// </summary>
    public string Content { get; set; } = default!;

    /// <summary>
    /// Дата создания.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// День последного изменение.
    /// </summary>
    public DateTime? EditedDate { get; protected set; }

    /// <summary>
    /// Идентификатор автора.
    /// </summary>
    public Guid AuthorId { get; set; }

    public void EditedAt(DateTime date)
    {
        EditedDate = date;
    }
}
