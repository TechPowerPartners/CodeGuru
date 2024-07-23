using Domain.Enums;

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
    /// Состояние.
    /// </summary>
    public ArticleState State { get; set; }

    /// <summary>
    /// Тэги.
    /// </summary>
    public ICollection<string> Tags { get; set; }

    /// <summary>
    /// Дата создания.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата опубликования.
    /// </summary>
    public DateTime PublichedAt { get; set; }

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

    /// <summary>
    /// Обновить состояние статьи на основе статьи из кэша.
    /// </summary>
    public void UpdateFromCache(Article cachedArticle)
    {
        Title = cachedArticle.Title;
        Content = cachedArticle.Content;
        Tags = cachedArticle.Tags;
    }
}
