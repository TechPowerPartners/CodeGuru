namespace Api.Contracts.Articles;

/// <summary>
/// Запрос для сохранения статьи пользователя.
/// </summary>
public class SaveUserArticleRequest
{
    /// <summary>
    /// Название.
    /// </summary>
    public string Title { get; set; } = default!;

    /// <summary>
    /// Содержимое.
    /// </summary>
    public string Content { get; set; } = default!;

    public string Description { get; set; }

    /// <summary>
    /// Тэги.
    /// </summary>
    public ICollection<string>? Tags { get; set; } = [];
}
