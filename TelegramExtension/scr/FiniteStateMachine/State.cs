namespace TelegramBotExtension.FiniteStateMachine;

public class State(long id) : IState
{
    public static IStorage Storage = new MemoryStorage();

    private readonly long _id = id;

    public Task SetState(string? state) => Storage.SetState(_id, state);

    public Task<string?> GetState() => Storage.GetState(_id);

    public Task UpdateData(string key, object value) => Storage.UpdateData(_id, key, value);

    public Task UpdateData(Dictionary<string, object> data) => Storage.UpdateData(_id, data);

    public Task SetData(Dictionary<string, object> data) => Storage.SetData(_id, data);

    public Task<Dictionary<string, object>> GetData() => Storage.GetData(_id);

    public Task Clear() => Storage.Clear(_id);
}
