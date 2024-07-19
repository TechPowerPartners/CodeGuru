namespace Api.Contracts.Articles;

/// <summary>
/// Запрос для создания статьи.
/// </summary>
public class CreateArticleRequest
{
    /// <summary>
    /// Название.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Содержимое.
    /// </summary>
    public string Content { get; set; } = default!;

    public bool CheckIfNull()
    {
        if (Title.Length < 1) return false;
        if (Content.Length < 1) return false;
        return true;
    }
}
