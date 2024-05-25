namespace Api.Contracts;

/// <summary>
/// Запрос постраничных данных
/// </summary>
/// <param name="Number">Номер страницы</param>
/// <param name="Size">Размер страницы</param>
public record PageRequest(int Number, int Size);
