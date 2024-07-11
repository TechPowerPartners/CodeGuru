namespace TelegramBotExtension.FiniteStateMachine;

internal interface IState
{
    Task SetState(string? state);
    Task<string?> GetState();
    Task UpdateData(string key, object value);
    Task UpdateData(Dictionary<string, object> data);
    Task SetData(Dictionary<string, object> data);
    Task<Dictionary<string, object>> GetData();
    Task Clear();
}
