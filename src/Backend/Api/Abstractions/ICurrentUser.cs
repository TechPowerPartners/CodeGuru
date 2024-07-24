using Domain.Entities;

namespace Api.Abstractions;

/// <summary>
/// Предоставляет доступ к текущему пользователю, если он авторизован.
/// </summary>
public interface ICurrentUser
{
    /// <summary>
    /// Текущий пользователь авторизован?
    /// </summary>
    public bool IsAuthenticated { get; }

    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid? Id { get; }

    /// <summary>
    /// Имя.
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Получить пользователя.
    /// </summary>
    public Task<User?> GetAsync();
}
