namespace Api.Contracts.AccountBindings;

public class BindTelegramAccountRequest
{
    public string Name { get; set; } = default!;
    public string Password { get; set; } = default!;
    public long TelegramUserId { get; set; }
}
