namespace Domain.Enums;

/// <summary>
/// Состояние статьи.
/// </summary>
public enum ArticleState
{
    /// <summary>
    /// Черновик.
    /// </summary>
    Draft = 0,

    /// <summary>
    /// На модерации.
    /// </summary>
    OnModeration = 1,

    /// <summary>
    /// Отклонена во время модерации.
    /// </summary>
    Rejected = 2,

    /// <summary>
    /// Опубликована.
    /// </summary>
    Published = 3
}
