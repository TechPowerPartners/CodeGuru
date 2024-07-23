namespace Api.Contracts.AccountBindings;

public class GetTelegramAccountBindingResponse
{
    public Guid UserId { get; set; }
    public long TelegramId { get; set; }
}
