namespace Api.Contracts.Articles;

/// <summary>
/// Запрос для сохранения черновика статьи.
/// </summary>
public class SaveDraftArticleRequest
{
    /// <summary>
    /// Название.
    /// </summary>
    public string Title { get; set; } = default!;

    /// <summary>
    /// Содержимое.
    /// </summary>
    public string Content { get; set; } = default!;

    /// <summary>
    /// Тэги.
    /// </summary>
    public ICollection<string> Tags { get; set; } = [];
}
