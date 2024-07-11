namespace TelegramBotExtension.FiniteStateMachine;

public interface IStorage
{
    Task SetState(long id, string? state);
    Task<string?> GetState(long id);
    Task UpdateData(long id, string key, object value);
    Task UpdateData(long id, Dictionary<string, object> data);
    Task SetData(long id, Dictionary<string, object> data);
    Task<Dictionary<string, object>> GetData(long id);
    Task Clear(long id);
}
